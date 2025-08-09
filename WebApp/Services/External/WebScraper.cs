using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Web;
using WebApp.Models;

namespace WebApp.Services.External
{
    public class WebScraper(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<Job>> GetJobsFromDescription(JobProvider provider, List<string> globalKeyWords)
        {
            Debug.WriteLine($"Getting jobs for provider {provider.Name} from description {provider.Descriptions.FirstOrDefault(p => p.Id == provider.Id)?.Name}");
            var request = new HttpRequestMessage(HttpMethod.Get, provider.Url!);
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/115.0.0.0 Safari/537.36");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var jobNodes = htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobTitle);
            var jobLocationNodes = htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobLocation);
            var jobDescriptionNodes = htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobDescription);
            var jobLinkNodes = htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobLink);

            List<int> Counts = [jobNodes.Count, jobLocationNodes.Count, jobDescriptionNodes.Count, jobLinkNodes.Count];

            if (Counts.Any(c => c != Counts[0]))
            {
                Debug.WriteLine($"Inconsistent number of scraped data {Counts[0]} {Counts[1]} {Counts[2]} {Counts[3]}");
                return [];
            }

            List<Job> Jobs = [];
            for (int i = 0; i < jobNodes.Count; i++)
            {
                Jobs.Add(new Job()
                {
                    Title = HttpUtility.HtmlDecode(jobNodes[i].InnerHtml),
                    Location = HttpUtility.HtmlDecode(jobLocationNodes[i].InnerHtml),
                    Description = HttpUtility.HtmlDecode(jobDescriptionNodes[i].InnerHtml),
                    Url = HttpUtility.HtmlDecode(jobLinkNodes[i].Attributes["href"].Value),
                    Found = DateTimeOffset.Now
                });
            }

            return Jobs;
        }
    }
}
