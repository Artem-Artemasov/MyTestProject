using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkFinder.DbWorker.Migrations
{
    public partial class add_twoStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InHtml",
                table: "Results",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InSitemap",
                table: "Results",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InHtml",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "InSitemap",
                table: "Results");
        }
    }
}
