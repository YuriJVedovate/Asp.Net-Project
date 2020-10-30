using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TalkToAPI.Migrations
{
    public partial class AddToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RefreshToken = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Used = table.Column<bool>(nullable: false),
                    ExpirationToken = table.Column<DateTime>(nullable: false),
                    ExpirationRefreshToken = table.Column<DateTime>(nullable: false),
                    Insert = table.Column<DateTime>(nullable: false),
                    Update = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tokens_UserId",
                table: "tokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");
        }
    }
}
