using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Migrations
{
    public partial class UpdatingRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

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
                name: "OrderStatus",
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
                name: "Email",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
