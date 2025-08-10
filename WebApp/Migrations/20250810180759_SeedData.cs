using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobProvider",
                columns: new[] { "Id", "Class_JobDescription", "Class_JobLink", "Class_JobLocation", "Class_JobTitle", "Class_NextPage", "KeyWords", "LastScraped", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "div.job-card__text > p", "a.job-card__link", "div.job-card__location", "div.job-card > h3", null, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "BAE Systems", "https://jobsearch.baesystems.com/search-and-apply" },
                    { 2, "div.job-card__text > p", "a.job-card__link", "div.job-card__location", "div.job-card > h3", null, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Revolut", "https://www.revolut.com/careers/" },
                    { 3, null, "div.jobs-title > b > a", "div.jobs-location > small", "a.jobs-title > b", "div.pager__item--next", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Nestle", "https://www.nestle.com/jobs/search-jobs" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobProvider",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JobProvider",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "JobProvider",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
