using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var bae = new JobProvider()
            {
                Id = 1,
                Name = "BAE Systems",
                Url = "https://jobsearch.baesystems.com/search-and-apply",
                Class_JobTitle = "div.job-card > h3",
                Class_JobLink = "a.job-card__link",
                Class_JobDescription = "div.job-card__text > p",
                Class_JobLocation = "div.job-card__location",
            };

            modelBuilder.Entity<JobProvider>().HasData(
                bae,
                new JobProvider()
                {
                    Id = 2,
                    Name = "Revolut",
                    Url = "https://www.revolut.com/careers/",
                    Class_JobTitle = "div.job-card > h3",
                    Class_JobLink = "a.job-card__link",
                    Class_JobDescription = "div.job-card__text > p",
                    Class_JobLocation = "div.job-card__location"
                },
                new JobProvider()
                {
                    Id = 3,
                    Name = "Nestle",
                    Url = "https://www.nestle.com/jobs/search-jobs",
                    Class_JobTitle = "a.jobs-title > b",
                    Class_JobLink = "div.jobs-title > b > a",
                    Class_JobLocation = "div.jobs-location > small",
                    Class_NextPage = "div.pager__item--next"
                }
            );

            modelBuilder.Entity<JobDescription>().HasData(
                new JobDescription() 
                {
                    Id = 1,
                    Name = "Software Engineer",
                    GlobalKeyWords = [ "junior", "entry", "grad" ],
                }
            );
        }

        public DbSet<WebApp.Models.Job> Job { get; set; } = default!;
        public DbSet<WebApp.Models.JobProvider> JobProvider { get; set; } = default!;
        public DbSet<WebApp.Models.JobDescription> JobDescription { get; set; } = default!;
    }
}
