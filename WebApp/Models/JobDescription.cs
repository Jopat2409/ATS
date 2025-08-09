using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class JobDescription
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<string>? Locations { get; set; }
        public ICollection<string>? GlobalKeyWords { get; set; }
        public ICollection<JobProvider> Providers { get; set; } = [];


        [NotMapped]
        public IEnumerable<Job> ActiveJobs => Providers
            .Where(p => p.ActiveJobs.Any())
            .SelectMany(p => p.ActiveJobs);

        [NotMapped]
        public IEnumerable<Job> NewJobs => Providers
            .SelectMany(p => p.NewJobs);

        [NotMapped]
        public IEnumerable<JobProvider> ToScrape => Providers
            .Where(p => p.LastScraped < DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(30)));

        [NotMapped]
        public IEnumerable<Job> ExpiredJobs => Providers
            .Where(p => p.ExpiredJobs.Any())
            .SelectMany(p => p.ExpiredJobs);

        [NotMapped]
        public Job? MostRecentJob => Providers
            .Where(p => p.ActiveJobs.Any())
            .SelectMany(p => p.ActiveJobs)
            .OrderByDescending(p => p.Found)
            .FirstOrDefault();

    }
}
