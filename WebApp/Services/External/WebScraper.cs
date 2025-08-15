using HtmlAgilityPack;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Web;
using WebApp.Models;

namespace WebApp.Services.External
{
    public class WebScraper(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private static HttpRequestMessage ConstructRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/115.0.0.0 Safari/537.36");

            request.Headers.Referrer = new Uri("https://google.com/");

            request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.5");
            request.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            return request;
        }

        public async Task<List<JobListing>> ScrapeListingsFromJob(Job job, JobSource source)
        {
            Debug.WriteLine($"Getting jobs for provider {source.Name} from description {job.Name}");
            Debug.WriteLine($"URI: {Uri.EscapeDataString(job.Name!)}");

            var request = ConstructRequest(source.Url! + Uri.EscapeDataString(job.Name!));

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var html = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var jobNodes = htmlDoc.DocumentNode.QuerySelectorAll(source.Selector_JobTitle);

            var jobLinkNodes = !string.IsNullOrEmpty(source.Selector_JobLink)
                ? htmlDoc.DocumentNode.QuerySelectorAll(source.Selector_JobLink)
                : null;

            var jobLocationNodes = !string.IsNullOrEmpty(source.Selector_JobLocation)
                ? htmlDoc.DocumentNode.QuerySelectorAll(source.Selector_JobLocation)
                : null;

            var jobDescriptionNodes = !string.IsNullOrEmpty(source.Selector_JobDescription)
                ? htmlDoc.DocumentNode.QuerySelectorAll(source.Selector_JobDescription)
                : null;

            List<int?> counts = [jobNodes.Count, jobLinkNodes?.Count, jobLocationNodes?.Count, jobDescriptionNodes?.Count];

            if (counts.Where(c => c != null).Any(c => c != counts[0]))
            {
                Debug.WriteLine($"Inconsistent number of scraped data {counts[0]} {counts[1]} {counts[2]} {counts[3]}");
                return [];
            }

            List<JobListing> Jobs = [];
            for (int i = 0; i < jobNodes.Count; i++)
            {
                Jobs.Add(new JobListing()
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
                    Found = DateTimeOffset.Now,
                    Source = source
                });
            }

            return Jobs;
        }

        public async Task<List<JobListing>> FetchListingsFromJob(Job job, JobSource source)
        {
            var request = ConstructRequest(source.Url! + Uri.EscapeDataString(job.Name!));

            var response = await _httpClient.SendAsync(request);


            var data = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(data);

            return [];
        }
    }
}
