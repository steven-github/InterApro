using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Migrations
{
    public partial class AddBossIdToRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BossId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BossId",
                table: "Requests");
        }
    }
}
