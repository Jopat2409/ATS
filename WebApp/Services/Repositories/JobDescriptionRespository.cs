using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobDescriptionRespository(IDbContextFactory<WebAppContext> db) : IAsyncRepository<JobDescription>
    {

        private readonly IDbContextFactory<WebAppContext> _contextFactory = db;

        public async Task<JobDescription> CreateAsync(JobDescription t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.JobDescription.Add(t);
            await context.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(JobDescription t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.JobDescription.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task<List<JobDescription>> ReadAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobDescription.Include(j => j.Providers).ThenInclude(p => p.Jobs).ToListAsync();
        }

        public async Task<List<JobDescription>> ReadAllAsync(Expression<Func<JobDescription, bool>> criteria)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobDescription.Where(criteria).Include(j => j.Providers).ThenInclude(p => p.Jobs).ToListAsync();
        }

        public async Task<JobDescription?> ReadOneAsync(int id)
        {
            return await ReadOneAsync(p => p.Id == id);
        }

        public async Task<JobDescription?> ReadOneAsync(Expression<Func<JobDescription, bool>> criteria)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.JobDescription.Include(j => j.Providers).ThenInclude(p => p.Jobs).FirstOrDefaultAsync(criteria);
        }

        public async Task AddProviderAsync(int id, JobProvider provider)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.JobDescription
                .Where(jd => jd.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Providers, p => p.Providers.Append(provider)
                ));

        }

        public async Task<JobDescription> UpdateAsync(JobDescription t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            JobDescription? scopedDescription = await context.JobDescription.Include(d => d.Providers).ThenInclude(p => p.Jobs).FirstOrDefaultAsync(d => d.Id == t.Id);

            if (scopedDescription == null) return await CreateAsync(t);

            Debug.WriteLine($"Adding Job Description {t.Name} with Id {t.Id}");

            context.Entry(scopedDescription).CurrentValues.SetValues(t);
            
            foreach (var provider in t.Providers)
            {
                var scopedProvider = scopedDescription.Providers.FirstOrDefault(p => p.Id == provider.Id);

                if (scopedProvider != null)
                {
                    context.Entry(scopedProvider).CurrentValues.SetValues(provider);

                    foreach (var job in provider.Jobs)
                    {
                        var scopedJob = scopedProvider.Jobs.FirstOrDefault(j => j.Id == job.Id);

                        if (scopedJob != null)
                        {
                            context.Entry(scopedJob).CurrentValues.SetValues(job);
                        } else
                        {
                            job.Provider = scopedProvider;
                            scopedProvider.Jobs.Add(job);
                        }
                    }

                } else
                {
                    scopedDescription.Providers.Add(provider);
                }
            }

            await context.SaveChangesAsync();
            return scopedDescription;
        }
    }
}
