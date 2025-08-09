using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLastScraped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "Class_JobCard",
               table: "JobProvider"
            );

            migrationBuilder.AddColumn<string>(
                name: "Class_NextPage",
                table: "JobProvider",
                type: "nvarchar(max)",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastScraped",
                table: "JobProvider",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1), TimeSpan.Zero)
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastScraped",
                table: "JobProvider"
            );

            migrationBuilder.DropColumn(
                name: "Class_NextPage",
                table: "JobProvider"
             );

            // Add back the old column you deleted
            migrationBuilder.AddColumn<string>(
                name: "Class_JobCard",
                table: "JobProvider",
                type: "nvarchar(max)",
                nullable: true
             );
        }
    }
}
