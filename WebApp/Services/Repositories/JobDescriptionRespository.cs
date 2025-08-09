using Microsoft.EntityFrameworkCore;
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

        public async Task<JobDescription> UpdateAsync(JobDescription t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.JobDescription.Update(t);
            await context.SaveChangesAsync();

            return t;
        }
    }
}
