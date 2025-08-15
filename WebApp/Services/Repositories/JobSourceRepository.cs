using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobSourceRepository(IDbContextFactory<WebAppContext> db)
    {
        private readonly IDbContextFactory<WebAppContext> _contextFactory = db;

        public async Task<JobSource> CreateAsync(JobSource source)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            await context.Source.AddAsync(source);
            await context.SaveChangesAsync();

            return source;
        }

        public Task DeleteAsync(JobSource t)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobSource>> ReadAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Source.Include(p => p.Listings).ToListAsync();
        }

        public Task<List<JobSource>> ReadAllAsync(Expression<Func<JobSource, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public async Task<JobSource?> ReadOneAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return context.Source.Include(p => p.Listings).FirstOrDefault(p => p.Id == id);
        }

        public Task<JobSource?> ReadOneAsync(Expression<Func<JobSource, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<JobSource> UpdateAsync(JobSource t)
        {
            throw new NotImplementedException();
        }

        public async Task<JobSource> AddListingsAsync(int providerId, ICollection<JobListing> jobs)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            JobSource provider = (await context.Source.Include(p => p.Listings).FirstOrDefaultAsync(p => p.Id == providerId))!;

            foreach (var job in jobs)
            {
                if (!provider.Listings.Any(j => j.Url == job.Url))
                {
                    job.Source = provider;
                    provider.Listings.Add(job);
                }
            }

            await context.SaveChangesAsync();

            return (await context.Source.FirstOrDefaultAsync(p => p.Id == providerId))!;
        }

        public async Task<JobSource> SetLastScrapedAsync(int providerId, DateTimeOffset time)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            JobSource provider = (await context.Source.FirstOrDefaultAsync(p => p.Id == providerId))!;

            provider.LastScraped = time;

            await context.SaveChangesAsync();

            return (await context.Source.FirstOrDefaultAsync(p => p.Id == providerId))!;
        }
    }
}
