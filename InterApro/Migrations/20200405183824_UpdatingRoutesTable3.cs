using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Migrations
{
    public partial class UpdatingRoutesTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssigneeName",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Requests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Requests");
        }
    }
}
