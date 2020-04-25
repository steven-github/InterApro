using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Web.Migrations
{
    public partial class Customize_Identity_Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxAmount",
                table: "AspNetRoles",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinAmout",
                table: "AspNetRoles",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "MinAmout",
                table: "AspNetRoles");
        }
    }
}
