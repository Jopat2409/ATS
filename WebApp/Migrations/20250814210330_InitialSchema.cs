using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyWords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locations = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    LastScraped = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Selector_JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Selector_JobLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selector_JobLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selector_JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selector_NextPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selector_JobListings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudflareBlocked = table.Column<bool>(type: "bit", nullable: false),
                    KeyWords = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobJobSource",
                columns: table => new
                {
                    DescriptionsId = table.Column<int>(type: "int", nullable: false),
                    SourcesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobJobSource", x => new { x.DescriptionsId, x.SourcesId });
                    table.ForeignKey(
                        name: "FK_JobJobSource_Job_DescriptionsId",
                        column: x => x.DescriptionsId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobJobSource_Source_SourcesId",
                        column: x => x.SourcesId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Listing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The URL to the job description"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The full name of the job description"),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    Applied = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the system has a logged application for this listing"),
                    Seen = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the user has viewed this application"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The location of the job"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The job description"),
                    Found = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, comment: "What time the job was scraped"),
                    Posted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "What time the job was posted"),
                    Expires = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "What time the job listing expires")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listing_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Source",
                columns: new[] { "Id", "CloudflareBlocked", "KeyWords", "LastScraped", "Name", "Selector_JobDescription", "Selector_JobLink", "Selector_JobListings", "Selector_JobLocation", "Selector_JobTitle", "Selector_NextPage", "SourceType", "Url" },
                values: new object[,]
                {
                    { 1, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "BAE Systems", "div.job-card__text > p", "a.job-card__link", null, "div.job-card__location", "div.job-card > h3", null, 0, "https://jobsearch.baesystems.com/search-and-apply?_search=" },
                    { 2, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Revolut", "div.job-card__text > p", "a.job-card__link", null, "div.job-card__location", "div.job-card > h3", null, 0, "https://www.revolut.com/careers/?text=" },
                    { 3, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Nestle UK", null, "div.jobs-title > b > a", null, "div.jobs-location > small", "a.jobs-title > b", "div.pager__item--next", 0, "https://www.nestle.com/jobs/search-jobs?keyword=" },
                    { 4, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ARM", null, "a.job-card__title", null, "span.location", "a.job-card__title", null, 0, "https://careers.arm.com/search-jobs/" },
                    { 5, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dassault Systemes UK", null, null, null, "div.job-card-place > div", "div.job-card-title", null, 0, "https://www.3ds.com/careers/jobs?wockw=" },
                    { 6, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Deliveroo", null, "a.post", null, "span.text-body.text-black-teal", "h4.text-body-lg.text-body-strong", null, 0, "https://careers.deliveroo.co.uk/join-the-team/?search=" },
                    { 7, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "IBM", null, "a.bx--card-group__card", null, null, "div.bx--card__heading", null, 0, "https://www.ibm.com/uk-en/careers/search?q=" },
                    { 8, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Sage UK", "div.function", "div.job-link", null, "div.office-location", "div.job-title", null, 0, "https://www.sage.com/en-gb/company/careers/career-search/?keywords=soft" },
                    { 9, false, "[]", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Sage API", "Description", "Url", "vacancies.Records", "OfficeLocation", "Name", null, 0, "https://www.sage.com/api/sagedotcom/careersearch/getcareersearchdata/" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobJobSource_SourcesId",
                table: "JobJobSource",
                column: "SourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Listing_SourceId",
                table: "Listing",
                column: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobJobSource");

            migrationBuilder.DropTable(
                name: "Listing");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Source");
        }
    }
}
