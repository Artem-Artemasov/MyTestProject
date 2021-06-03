using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkFinder.DbWorker.Migrations
{
    public partial class add_timeCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Tests");
        }
    }
}
