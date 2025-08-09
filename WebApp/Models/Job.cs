using System.ComponentModel;

namespace WebApp.Models
{
    public class Job
    {

        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Title { get; set; }


        public int ApplicationProviderId { get; set; }
        public JobProvider? Provider;

        [DefaultValue(false)]
        public bool Applied { get; set; }

        [DefaultValue(false)]
        public bool Seen { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? Posted { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public DateTimeOffset Found { get; set; }

    }
}
