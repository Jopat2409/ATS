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
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/115.0.0.0 Safari/537.36");
            request.Headers.Referrer = new Uri("https://example.com/");
            request.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.5");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var html = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var jobNodes = htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobTitle);

            var jobLinkNodes = !string.IsNullOrEmpty(provider.Class_JobLink)
                ? htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobLink)
                : null;

            var jobLocationNodes = !string.IsNullOrEmpty(provider.Class_JobLocation)
                ? htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobLocation)
                : null;

            var jobDescriptionNodes = !string.IsNullOrEmpty(provider.Class_JobDescription)
                ? htmlDoc.DocumentNode.QuerySelectorAll(provider.Class_JobDescription)
                : null;

            List<int?> counts = [jobNodes.Count, jobLinkNodes?.Count, jobLocationNodes?.Count, jobDescriptionNodes?.Count];

            if (counts.Where(c => c != null).Any(c => c != counts[0]))
            {
                Debug.WriteLine($"Inconsistent number of scraped data {counts[0]} {counts[1]} {counts[2]} {counts[3]}");
                return [];
            }

            List<Job> Jobs = [];
            for (int i = 0; i < jobNodes.Count; i++)
            {
                Jobs.Add(new Job()
                {
                    Title = HttpUtility.HtmlDecode(jobNodes[i].InnerHtml),
                    Location = jobLocationNodes != null
                        ? HttpUtility.HtmlDecode(jobLocationNodes[i].InnerHtml)
                        : null,
                    Description = jobDescriptionNodes != null
                        ? HttpUtility.HtmlDecode(jobDescriptionNodes[i].InnerHtml)
                        : null,
                    Url = jobLinkNodes != null
                        ? HttpUtility.HtmlDecode(jobLinkNodes[i].Attributes["href"].Value)
                        : null,
                    Found = DateTimeOffset.Now
                });
            }

            return Jobs;
        }
    }
}
