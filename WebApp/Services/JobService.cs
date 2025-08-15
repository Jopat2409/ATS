using System.Diagnostics;
using WebApp.Models;
using WebApp.Services.External;
using WebApp.Services.Repositories;

namespace WebApp.Services
{
    public class JobService(
        WebScraper scraper, 
        JobRepository repo, 
        JobSourceRepository providerRepo, 
        ILogger<JobService> logger)
    {

        private readonly WebScraper _scraper = scraper;
        private readonly JobRepository _repo = repo;
        private readonly ILogger<JobService> _logger = logger;
        private readonly JobSourceRepository _providerRepo = providerRepo;

        public async Task<ICollection<JobListing>> GetJobListings(Job job)
        {
            _logger.LogInformation("Fetching all jobs for {job}", job.Name);

            _logger.LogInformation("Scraped {} new jobs for {job}", await ScrapeListings(job), job.Name);
            _logger.LogInformation("Found {} new jobs for {job} through API access", await RequestAPIs(job), job.Name);

            return await _repo.ReadListingsAsync(job.Id);
        }

        private async Task<int> ScrapeListings(Job job)
        {

            int total = 0;

            foreach (var source in job.ToScrape)
            {
                var jobs = await _scraper.ScrapeListingsFromJob(job, source);

                if (jobs.Count == 0)
                {
                    _logger.LogWarning("{p} is likely blocked by Cloudflare or CORS policy", source.Name);
                    source.CloudflareBlocked = true;
                }
                else
                {
                    _logger.LogInformation("Found {count} new listings from {name}", jobs.Count, source.Name);
                    await _providerRepo.AddListingsAsync(source.Id, jobs);
                }

                total += jobs.Count;
                await _providerRepo.SetLastScrapedAsync(source.Id, DateTimeOffset.Now);
            }
            return total;
        }

        private async Task<int> RequestAPIs(Job job)
        {
            int total = 0;

            foreach (var source in job.ToFetch)
            {
                var jobs = await _scraper.FetchListingsFromJob(job, source);

                if (jobs.Count == 0)
                {
                    _logger.LogWarning("{p} is likely blocked by Cloudflare or CORS policy", source.Name);
                    source.CloudflareBlocked = true;
                }
                else
                {
                    _logger.LogInformation("Found {count} new listings from {name}", jobs.Count, source.Name);
                    await _providerRepo.AddListingsAsync(source.Id, jobs);
                }
            }

            return total;
        }

        public async Task<ICollection<JobListing>> GetJobs(int id)
        {
            Job? description = await _repo.ReadOneAsync(id);

            if (description == null) return [];

            return await GetJobListings(description);
        }

    }
}
