using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobListingRepository(
        IDbContextFactory<WebAppContext> factory,
        ILogger<JobListingRepository> logger)
    {

        private readonly ILogger<JobListingRepository> _logger = logger;
        private readonly IDbContextFactory<WebAppContext> _contextFactory = factory;

        public enum Include
        {
            NONE,
            SOURCE,
        }

        public async Task<JobListing> CreateAsync(JobListing t)
        {
            _logger.LogInformation("Creating listing {listing}", t.Title);

            using var context = await _contextFactory.CreateDbContextAsync();
            context.Listing.Add(t);
            await context.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(JobListing t)
        {
            _logger.LogInformation("Deleting listing {listing}", t.Title);
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Listing.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task<List<JobListing>> ReadAllAsync(Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCE => await context.Listing.Include(j => j.Source).ToListAsync(),
                _ => await context.Listing.ToListAsync()
            };
        }

        public async Task<List<JobListing>> ReadAllAsync(
            Expression<Func<JobListing, bool>> criteria,
            Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCE => await context.Listing.Include(j => j.Source).Where(criteria).ToListAsync(),
                _ => await context.Listing.Where(criteria).ToListAsync()
            };
        }

        public async Task<JobListing?> ReadOneAsync(int id, Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCE => await context.Listing.Include(l => l.Source).FirstOrDefaultAsync(p => p.Id == id),
                _ => await context.Listing.FirstOrDefaultAsync(p => p.Id == id)
            };
        }

        public async Task<JobListing?> ReadOneAsync(
            Expression<Func<JobListing, bool>> criteria,
            Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCE => await context.Listing.Include(l => l.Source).FirstOrDefaultAsync(criteria),
                _ => await context.Listing.FirstOrDefaultAsync(criteria)
            };
        }

        public async Task<JobListing> UpdateAsync(JobListing t)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Listing.Update(t);
            await context.SaveChangesAsync();

            return t;
        }
    }
}
