using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IranOtaku.Data.Migrations
{
    public partial class AddChapterPay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimePayments");

            migrationBuilder.DropTable(
                name: "BookPayments");

            migrationBuilder.CreateTable(
                name: "ChapterPayments",
                columns: table => new
                {
                    PayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterPayments", x => x.PayId);
                    table.ForeignKey(
                        name: "FK_ChapterPayments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterPayments_BookChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "BookChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpsodePayments",
                columns: table => new
                {
                    PayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EpsodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpsodePayments", x => x.PayId);
                    table.ForeignKey(
                        name: "FK_EpsodePayments_AnimeEpsodes_EpsodeId",
                        column: x => x.EpsodeId,
                        principalTable: "AnimeEpsodes",
                        principalColumn: "EpsodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpsodePayments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubtitlePayments",
                columns: table => new
                {
                    PayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubtitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubtitlePayments", x => x.PayId);
                    table.ForeignKey(
                        name: "FK_SubtitlePayments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubtitlePayments_Subtitles_SubtitleId",
                        column: x => x.SubtitleId,
                        principalTable: "Subtitles",
                        principalColumn: "SubtitleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterPayments_ChapterId",
                table: "ChapterPayments",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterPayments_UserId",
                table: "ChapterPayments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EpsodePayments_EpsodeId",
                table: "EpsodePayments",
                column: "EpsodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EpsodePayments_UserId",
                table: "EpsodePayments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubtitlePayments_SubtitleId",
                table: "SubtitlePayments",
                column: "SubtitleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubtitlePayments_UserId",
                table: "SubtitlePayments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterPayments");

            migrationBuilder.DropTable(
                name: "EpsodePayments");

            migrationBuilder.DropTable(
                name: "SubtitlePayments");

            migrationBuilder.CreateTable(
                name: "AnimePayments",
                columns: table => new
                {
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimePayments", x => x.PayTime);
                    table.ForeignKey(
                        name: "FK_AnimePayments_Animes_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookPayments",
                columns: table => new
                {
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPayments", x => x.PayTime);
                    table.ForeignKey(
                        name: "FK_BookPayments_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimePayments_AnimeId",
                table: "AnimePayments",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPayments_BookId",
                table: "BookPayments",
                column: "BookId");
        }
    }
}
