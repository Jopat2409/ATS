using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        public ICollection<string> KeyWords { get; set; } = [];
        public ICollection<string> Locations { get; set; } = [];
        public ICollection<JobSource> Sources { get; set; } = [];

        public static Job Empty
        {
            get => new()
            {
                Name = string.Empty,
                KeyWords = [],
                Locations = [],
                Sources = []
            };
            set => throw new NotImplementedException("Why are you trying to set an empty job :(");
        }

        [NotMapped]
        public IEnumerable<JobListing> AllListings => Sources
            .SelectMany(p => p.Listings);

        [NotMapped]
        public IEnumerable<JobListing> ActiveListings => Sources
            .Where(p => p.ActiveListings.Any())
            .SelectMany(p => p.ActiveListings);

        [NotMapped]
        public IEnumerable<JobListing> NewListings => Sources
            .SelectMany(p => p.NewListings);

        [NotMapped]
        public IEnumerable<JobSource> ToScrape => Sources
            .Where(p => p.LastScraped < DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(30)) && p.SourceType == JobSourceType.COMPANY);

        [NotMapped]
        public IEnumerable<JobSource> ToFetch => Sources
            .Where(p => p.LastScraped < DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(30)) && p.SourceType == JobSourceType.API);

        [NotMapped]
        public IEnumerable<JobListing> ExpiredListings => Sources
            .Where(p => p.ExpiredListings.Any())
            .SelectMany(p => p.ExpiredListings);

        [NotMapped]
        public JobListing? MostRecentJob => Sources
            .Where(p => p.ActiveListings.Any())
            .SelectMany(p => p.ActiveListings)
            .OrderByDescending(p => p.Found)
            .FirstOrDefault();

    }
}
