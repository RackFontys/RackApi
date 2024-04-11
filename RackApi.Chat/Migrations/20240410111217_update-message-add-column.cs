using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackApi.Chat.Migrations
{
    /// <inheritdoc />
    public partial class updatemessageaddcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToUserId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Messages");
        }
    }
}
