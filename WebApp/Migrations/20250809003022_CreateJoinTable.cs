using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobProvider_JobDescription_JobDescriptionId",
                table: "JobProvider");

            migrationBuilder.DropIndex(
                name: "IX_JobProvider_JobDescriptionId",
                table: "JobProvider");

            migrationBuilder.DropColumn(
                name: "JobDescriptionId",
                table: "JobProvider");

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

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptionJobProvider_ProvidersId",
                table: "JobDescriptionJobProvider",
                column: "ProvidersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobDescriptionJobProvider");

            migrationBuilder.AddColumn<int>(
                name: "JobDescriptionId",
                table: "JobProvider",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobProvider_JobDescriptionId",
                table: "JobProvider",
                column: "JobDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobProvider_JobDescription_JobDescriptionId",
                table: "JobProvider",
                column: "JobDescriptionId",
                principalTable: "JobDescription",
                principalColumn: "Id");
        }
    }
}
