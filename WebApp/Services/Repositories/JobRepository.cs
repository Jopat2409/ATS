using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services.Repositories
{
    public class JobRepository(
        ILogger<JobRepository> logger,
        IDbContextFactory<WebAppContext> factory)
    {

        private readonly ILogger<JobRepository> _logger = logger;
        private readonly IDbContextFactory<WebAppContext> _contextFactory = factory;

        public enum Include
        {
            NONE,
            SOURCES,
            LISTINGS
        }

        public async Task<Job> CreateAsync(Job t)
        {
            _logger.LogInformation("Creating new job {j}", t.Name);
            using var context = await _contextFactory.CreateDbContextAsync();

            context.Job.Add(t);

            // Dont insert existing sources ya dummy
            foreach (var source in t.Sources)
            {
                if (source.Id != 0) context.Entry(source).State = EntityState.Unchanged;
            }

            await context.SaveChangesAsync();

            return t;
        }

        public async Task CopySourcesAsync(int from, int to)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            Job? jobFrom = context.Job.Include(j => j.Sources).FirstOrDefault(j => j.Id == from);
            Job? jobTo = context.Job.Include(j => j.Sources).FirstOrDefault(j => j.Id == to);

            if (jobFrom == null || jobTo == null)
            {
                _logger.LogError("One of jobs {} (source) or {} (destination) does not exist", from, to);
                return;
            }

            _logger.LogInformation("Copying sources from {} to {}", jobFrom.Name, jobTo.Name);

            foreach (var source in jobFrom.Sources)
            {
                if (!jobTo.Sources.Any(s => s.Id == source.Id)) jobTo.Sources.Add(source);
            }

            await context.SaveChangesAsync();
        }

        public async Task<Job> CreateAsync(
            string name,
            List<string>? keyWords = default,
            List<string>? locations = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var job = new Job()
            {
                Name = name,
                KeyWords = keyWords ?? [],
                Locations = locations ?? []
            };

            context.Job.Add(job);
            await context.SaveChangesAsync();

            return job;
        }

        public async Task DeleteAsync(Job t)
        {
            _logger.LogInformation("Deleting job {name} with ID {id}", t.Name, t.Id);
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Job.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task<List<Job>> ReadAllAsync(
            Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCES => await context
                    .Job
                    .Include(j => j.Sources)
                    .ToListAsync(),
                Include.LISTINGS => await context
                    .Job
                    .Include(j => j.Sources)
                    .ThenInclude(s => s.Listings)
                    .ToListAsync(),
                _ => await context
                    .Job
                    .ToListAsync(),
            };
        }

        public async Task<List<Job>> ReadAllAsync(
            Expression<Func<Job, bool>> criteria,
            Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var filtered = context.Job.Where(criteria);
            _logger.LogInformation("Found {} jobs with filter {}", filtered.Count(), criteria);

            return include switch
            {
                Include.SOURCES => await filtered
                    .Include(j => j.Sources)
                    .ToListAsync(),
                Include.LISTINGS => await filtered
                    .Include(j => j.Sources)
                    .ThenInclude(s => s.Listings)
                    .ToListAsync(),
                _ => await filtered
                    .ToListAsync(),
            };
        }

        public async Task<Job?> ReadOneAsync(
            Expression<Func<Job, bool>> criteria, 
            Include include = Include.NONE)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return include switch
            {
                Include.SOURCES => await context
                    .Job
                    .Include(j => j.Sources)
                    .FirstOrDefaultAsync(criteria),
                Include.LISTINGS => await context
                    .Job
                    .Include(j => j.Sources)
                    .ThenInclude(p => p.Listings)
                    .FirstOrDefaultAsync(criteria),
                _ => await context
                    .Job
                    .FirstOrDefaultAsync(criteria),
            } ;
        }

        public async Task<Job?> ReadOneAsync(int id, Include include = Include.NONE)
        {
            return await ReadOneAsync(p => p.Id == id, include);
        }


        public async Task AddExistingSourceAsync(int id, int sourceId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            Job job = (await context.Job.FirstOrDefaultAsync(jd => jd.Id == id))!;
            JobSource source = (await context.Source.FirstOrDefaultAsync(p => p.Id == sourceId))!;

            _logger.LogInformation("Adding source {source} to job {job}", source.Name, job.Name);

            job.Sources.Add(source);

            await context.SaveChangesAsync();
        }

        public async Task RemoveExistingSourceAsync(int id, int sourceId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            Job job = (await context.Job.Include(j => j.Sources).FirstOrDefaultAsync(j => j.Id == id))!;
            JobSource source = (await context.Source.FirstOrDefaultAsync(s => s.Id == sourceId))!;

            _logger.LogInformation("Removing source {p} from job {i}", source.Name, job.Name);

            job.Sources.Remove(source);

            await context.SaveChangesAsync();
        }

        public async Task<List<JobListing>> ReadListingsAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            Job? job = await context
                .Job
                .Include(j => j.Sources)
                .ThenInclude(s => s.Listings)
                .FirstOrDefaultAsync(j => j.Id == id);

            return job == null 
                ? throw new NullReferenceException("Bad shit") 
                : [.. job.Sources.SelectMany(s => s.Listings)];
        }

        public async Task<Job?> UpdateDetailsAsync(Job updated)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            Job? job = await context.Job
                .FirstOrDefaultAsync(j => j.Id == updated.Id);

            if (job == null) {
                _logger.LogError("Attempted to update a Job that didn't exist (ID: {})", updated.Id);
                return null;
            }

            job.Name = updated.Name;
            job.KeyWords = updated.KeyWords;
            job.Locations = updated.Locations;

            job.Sources.Clear();

            foreach (var source in job.Sources)
            {
                JobSource? existing = await context.Source.FirstOrDefaultAsync(s => s.Id == source.Id);
                job.Sources.Add(existing ?? source);
            }

            await context.SaveChangesAsync();

            return job;
        }
    }
}
