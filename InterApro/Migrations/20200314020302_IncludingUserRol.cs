using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Migrations
{
    public partial class IncludingUserRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "User");
        }
    }
}
