using Microsoft.EntityFrameworkCore;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class JobListing
    {

        public int Id { get; set; }

        [Comment("The URL to the job description")]
        public string? Url { get; set; }

        [Comment("The full name of the job description")]
        public string? Title { get; set; }

        [Required]
        public required JobSource Source { get; set; }

        [Required]
        [DefaultValue(false)]
        [Comment("Whether the system has a logged application for this listing")]
        public bool Applied { get; set; }

        [Required]
        [DefaultValue(false)]
        [Comment("Whether the user has viewed this application")]
        public bool Seen { get; set; }

        [Comment("The location of the job")]
        public string? Location { get; set; }

        [Comment("The job description")]
        public string? Description { get; set; }

        [Comment("What time the job was scraped")]
        public DateTimeOffset Found { get; set; }

        [Comment("What time the job was posted")]
        public DateTimeOffset? Posted { get; set; }

        [Comment("What time the job listing expires")]
        public DateTimeOffset? Expires { get; set; }

        public override string ToString()
        {
            return $"{Title} ({Id})";
        }

    }
}
