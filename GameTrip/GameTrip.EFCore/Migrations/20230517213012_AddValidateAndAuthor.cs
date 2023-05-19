using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddValidateAndAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Picture",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsValidate",
                table: "Picture",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Location",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Location",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Game",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsValidate",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidate",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Picture_AuthorId",
                table: "Picture",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_AuthorId",
                table: "Location",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_AuthorId",
                table: "Game",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_AspNetUsers_AuthorId",
                table: "Game",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_AspNetUsers_AuthorId",
                table: "Location",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_AspNetUsers_AuthorId",
                table: "Picture",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_AspNetUsers_AuthorId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_AspNetUsers_AuthorId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_AspNetUsers_AuthorId",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Picture_AuthorId",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Location_AuthorId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Game_AuthorId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "IsValidate",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "IsValidate",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "IsValidate",
                table: "Comment");
        }
    }
}
