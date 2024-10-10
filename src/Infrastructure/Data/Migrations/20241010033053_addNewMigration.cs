using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addNewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetCode",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiration",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImgUrlId",
                table: "Shops",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_ImgUrlId",
                table: "Shops",
                column: "ImgUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Images_ImgUrlId",
                table: "Shops",
                column: "ImgUrlId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Images_ImgUrlId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Shops_ImgUrlId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "PasswordResetCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpiration",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImgUrlId",
                table: "Shops");
        }
    }
}
