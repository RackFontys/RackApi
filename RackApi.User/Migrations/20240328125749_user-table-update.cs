using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackApi.User.Migrations
{
    /// <inheritdoc />
    public partial class usertableupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverNb",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drivers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Drivers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Drivers",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Drivers",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Drivers",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Drivers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Drivers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Drivers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "companyId",
                table: "Drivers",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Drivers",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "DriverNb",
                table: "Drivers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
