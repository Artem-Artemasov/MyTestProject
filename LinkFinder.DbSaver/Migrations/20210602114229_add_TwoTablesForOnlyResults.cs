using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkFinder.DbSaver.Migrations
{
    public partial class add_TwoTablesForOnlyResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlyHtml",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlyHtml", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlySitemap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlySitemap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlyHtmlResult",
                columns: table => new
                {
                    OnlyHtmlId = table.Column<int>(type: "int", nullable: false),
                    ResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlyHtmlResult", x => new { x.OnlyHtmlId, x.ResultsId });
                    table.ForeignKey(
                        name: "FK_OnlyHtmlResult_OnlyHtml_OnlyHtmlId",
                        column: x => x.OnlyHtmlId,
                        principalTable: "OnlyHtml",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlyHtmlResult_Results_ResultsId",
                        column: x => x.ResultsId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlySitemapResult",
                columns: table => new
                {
                    OnlySitemapId = table.Column<int>(type: "int", nullable: false),
                    ResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlySitemapResult", x => new { x.OnlySitemapId, x.ResultsId });
                    table.ForeignKey(
                        name: "FK_OnlySitemapResult_OnlySitemap_OnlySitemapId",
                        column: x => x.OnlySitemapId,
                        principalTable: "OnlySitemap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlySitemapResult_Results_ResultsId",
                        column: x => x.ResultsId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlyHtmlResult_ResultsId",
                table: "OnlyHtmlResult",
                column: "ResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlySitemapResult_ResultsId",
                table: "OnlySitemapResult",
                column: "ResultsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlyHtmlResult");

            migrationBuilder.DropTable(
                name: "OnlySitemapResult");

            migrationBuilder.DropTable(
                name: "OnlyHtml");

            migrationBuilder.DropTable(
                name: "OnlySitemap");
        }
    }
}
