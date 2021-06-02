using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkFounder.DbSaver.Migrations
{
    public partial class add_TimeCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreated",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "CONVERT(date,GETUTCDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Tests");
        }
    }
}
