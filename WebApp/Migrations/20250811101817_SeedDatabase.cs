using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GlobalKeyWords = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobProvider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_NextPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudflareBlocked = table.Column<bool>(type: "bit", nullable: false),
                    LastScraped = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    KeyWords = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationProviderId = table.Column<int>(type: "int", nullable: false),
                    Applied = table.Column<bool>(type: "bit", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Posted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Expires = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Found = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    JobProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_JobProvider_JobProviderId",
                        column: x => x.JobProviderId,
                        principalTable: "JobProvider",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptionJobProvider",
                columns: table => new
                {
                    DescriptionsId = table.Column<int>(type: "int", nullable: false),
                    ProvidersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptionJobProvider", x => new { x.DescriptionsId, x.ProvidersId });
                    table.ForeignKey(
                        name: "FK_JobDescriptionJobProvider_JobDescription_DescriptionsId",
                        column: x => x.DescriptionsId,
                        principalTable: "JobDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobDescriptionJobProvider_JobProvider_ProvidersId",
                        column: x => x.ProvidersId,
                        principalTable: "JobProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobDescription",
                columns: new[] { "Id", "GlobalKeyWords", "Locations", "Name" },
                values: new object[] { 1, "[\"junior\",\"entry\",\"grad\"]", null, "Software Engineer" });

            migrationBuilder.InsertData(
                table: "JobProvider",
                columns: new[] { "Id", "Class_JobDescription", "Class_JobLink", "Class_JobLocation", "Class_JobTitle", "Class_NextPage", "CloudflareBlocked", "KeyWords", "LastScraped", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "div.job-card__text > p", "a.job-card__link", "div.job-card__location", "div.job-card > h3", null, false, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "BAE Systems", "https://jobsearch.baesystems.com/search-and-apply" },
                    { 2, "div.job-card__text > p", "a.job-card__link", "div.job-card__location", "div.job-card > h3", null, false, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Revolut", "https://www.revolut.com/careers/" },
                    { 3, null, "div.jobs-title > b > a", "div.jobs-location > small", "a.jobs-title > b", "div.pager__item--next", false, null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Nestle", "https://www.nestle.com/jobs/search-jobs" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobProviderId",
                table: "Job",
                column: "JobProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptionJobProvider_ProvidersId",
                table: "JobDescriptionJobProvider",
                column: "ProvidersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "JobDescriptionJobProvider");

            migrationBuilder.DropTable(
                name: "JobDescription");

            migrationBuilder.DropTable(
                name: "JobProvider");
        }
    }
}
