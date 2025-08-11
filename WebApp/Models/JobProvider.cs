using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{

    public class JobProvider
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Url { get; set; }

        public string? Class_JobLocation { get; set; }
        public string? Class_JobDescription { get; set; }
        public string? Class_JobTitle { get; set; }
        public string? Class_JobLink { get; set; }
        public string? Class_NextPage { get; set; }

        [DefaultValue(false)]
        public bool CloudflareBlocked { get; set; }

        public DateTimeOffset LastScraped { get; set; }

        public ICollection<Job> Jobs { get; set; } = [];
        public ICollection<string>? KeyWords { get; set; }

        public ICollection<JobDescription> Descriptions { get; set; } = [];

        [NotMapped]
        public IEnumerable<Job> ActiveJobs => [.. Jobs
            .Where(j => j.Expires > DateTimeOffset.Now)];

        [NotMapped]
        public IEnumerable<Job> ExpiredJobs => [.. Jobs
            .Where(j => j.Expires <= DateTime.Now)];

        [NotMapped]
        public IEnumerable<Job> NewJobs => [.. Jobs
            .Where(j => !j.Seen)];
    }
}
