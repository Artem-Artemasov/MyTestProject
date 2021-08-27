using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkFinder.DbWorker.Migrations
{
    public partial class add_TimeCreatedAndPropiertyForSeparateResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

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
                name: "TimeCreated",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "InHtml",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "InSitemap",
                table: "Results");
        }
    }
}
