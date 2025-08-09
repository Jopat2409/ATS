using NuGet.Packaging;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Services.External;
using WebApp.Services.Repositories;

namespace WebApp.Services
{
    public class JobDescriptionService(JobDescriptionRespository repo, WebScraper scraper)
    {

        private readonly JobDescriptionRespository _repo = repo;
        private readonly WebScraper _scraper = scraper;

        public async Task<ICollection<Job>> GetJobs(JobDescription description)
        {

            foreach (var provider in description.ToScrape)
            {
                Debug.WriteLine($"Getting jobs for provider {provider.Name} from description {provider.Descriptions.FirstOrDefault(p => p.Id == provider.Id)?.Name}");
                provider.Jobs.AddRange(await _scraper.GetJobsFromDescription(provider, description.GlobalKeyWords == null ? [] : [.. description.GlobalKeyWords]));
                provider.LastScraped = DateTimeOffset.Now;
            }

            await _repo.UpdateAsync(description);

            return [.. description.Providers.SelectMany(p => p.Jobs)];
        }

        public async Task<ICollection<Job>> GetJobs(int id)
        {
            JobDescription? description = await _repo.ReadOneAsync(id);

            if (description == null) return [];

            return await GetJobs(description);
        }

    }
}
