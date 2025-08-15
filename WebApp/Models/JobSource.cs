using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{

    public enum JobSourceType
    {
        COMPANY,
        BOARD,
        API
    }

    public class JobSource
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Url { get; set; }
        public JobSourceType SourceType { get; set; }

        public DateTimeOffset LastScraped { get; set; }

        public required string Selector_JobTitle { get; set; }

        public string? Selector_JobLocation { get; set; }
        public string? Selector_JobLink { get; set; }
        public string? Selector_JobDescription { get; set; }
        public string? Selector_NextPage { get; set; }
        public string? Selector_JobListings { get; set; }

        [DefaultValue(false)]
        public bool CloudflareBlocked { get; set; }

        public ICollection<string> KeyWords { get; set; } = [];
        public ICollection<Job> Descriptions { get; set; } = [];
        public ICollection<JobListing> Listings { get; set; } = [];

        public static JobSource Empty
        {
            get => new()
            {
                Name = string.Empty,
                Url = string.Empty,
                Selector_JobTitle = string.Empty
            };
        }


        [NotMapped]
        public IEnumerable<JobListing> ActiveListings => [.. Listings
            .Where(j => j.Expires > DateTimeOffset.Now)];

        [NotMapped]
        public IEnumerable<JobListing> ExpiredListings => [.. Listings
            .Where(j => j.Expires <= DateTime.Now)];

        [NotMapped]
        public IEnumerable<JobListing> NewListings => [.. Listings
            .Where(j => !j.Seen)];
    }
}
