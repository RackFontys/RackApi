using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackApi.Chat.Migrations
{
    /// <inheritdoc />
    public partial class updatemessagecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Messages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "messageText",
                table: "Messages",
                newName: "message");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ReadStatus",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReadStatus",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Messages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Messages",
                newName: "messageText");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Messages",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
