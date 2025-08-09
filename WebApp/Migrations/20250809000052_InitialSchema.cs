using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
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
                    GlobalKeyWords = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Class_JobCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Class_JobLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyWords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDescriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobProvider_JobDescription_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "JobDescription",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobProviderId",
                table: "Job",
                column: "JobProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_JobProvider_JobDescriptionId",
                table: "JobProvider",
                column: "JobDescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "JobProvider");

            migrationBuilder.DropTable(
                name: "JobDescription");
        }
    }
}
