using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Migrations
{
    public partial class IncludingBossIdOnUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BossId",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "BossId",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BossId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "BossId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
