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

            modelBuilder.Entity<JobSource>().HasData(
                new JobSource()
                {
                    Id = 1,
                    Name = "BAE Systems",
                    Url = "https://jobsearch.baesystems.com/search-and-apply?_search=",
                    Selector_JobTitle = "div.job-card > h3",
                    Selector_JobLink = "a.job-card__link",
                    Selector_JobDescription = "div.job-card__text > p",
                    Selector_JobLocation = "div.job-card__location",
                },
                new JobSource()
                {
                    Id = 2,
                    Name = "Revolut",
                    Url = "https://www.revolut.com/careers/?text=",
                    Selector_JobTitle = "div.job-card > h3",
                    Selector_JobLink = "a.job-card__link",
                    Selector_JobDescription = "div.job-card__text > p",
                    Selector_JobLocation = "div.job-card__location"
                },
                new JobSource()
                {
                    Id = 3,
                    Name = "Nestle UK",
                    Url = "https://www.nestle.com/jobs/search-jobs?keyword=",
                    Selector_JobTitle = "a.jobs-title > b",
                    Selector_JobLink = "div.jobs-title > b > a",
                    Selector_JobLocation = "div.jobs-location > small",
                    Selector_NextPage = "div.pager__item--next"
                },
                new JobSource()
                {
                    Id = 4,
                    Name = "ARM",
                    Url = "https://careers.arm.com/search-jobs/",
                    Selector_JobLocation = "span.location",
                    Selector_JobTitle = "a.job-card__title",
                    Selector_JobLink = "a.job-card__title"
                },
                new JobSource()
                {
                    Id = 5,
                    Name = "Dassault Systemes UK",
                    Url = "https://www.3ds.com/careers/jobs?wockw=",
                    Selector_JobLocation = "div.job-card-place > div",
                    Selector_JobTitle = "div.job-card-title"
                },
                new JobSource()
                {
                    Id = 6,
                    Name = "Deliveroo",
                    Url = "https://careers.deliveroo.co.uk/join-the-team/?search=",
                    Selector_JobLocation = "span.text-body.text-black-teal",
                    Selector_JobTitle = "h4.text-body-lg.text-body-strong",
                    Selector_JobLink = "a.post"
                },
                new JobSource()
                {
                    Id = 7,
                    Name = "IBM",
                    Url = "https://www.ibm.com/uk-en/careers/search?q=",
                    Selector_JobTitle = "div.bx--card__heading",
                    Selector_JobLink = "a.bx--card-group__card"
                },
                new JobSource()
                {
                    Id = 8,
                    Name = "Sage UK",
                    Url = "https://www.sage.com/en-gb/company/careers/career-search/?keywords=soft",
                    Selector_JobLocation = "div.office-location",
                    Selector_JobDescription = "div.function",
                    Selector_JobTitle = "div.job-title",
                    Selector_JobLink = "div.job-link"
                },
                new JobSource()
                {
                    Id = 9,
                    Name = "Sage API",
                    Url = "https://www.sage.com/api/sagedotcom/careersearch/getcareersearchdata/",
                    Selector_JobListings = "vacancies.Records",
                    Selector_JobTitle = "Name",
                    Selector_JobDescription = "Description",
                    Selector_JobLink = "Url",
                    Selector_JobLocation = "OfficeLocation"
                });
        }

        public DbSet<WebApp.Models.Job> Job { get; set; } = default!;
        public DbSet<WebApp.Models.JobSource> Source { get; set; } = default!;
        public DbSet<WebApp.Models.JobListing> Listing { get; set; } = default!;
    }
}
