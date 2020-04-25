using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InterApro.Database.Migrations
{
    public partial class CreateRequestSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestStatus",
                columns: table => new
                {
                    RequestStatusId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatus", x => x.RequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    RequestId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestDescription = table.Column<string>(maxLength: 4096, nullable: true),
                    RequestAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FinanceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestStatusId = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Request_RequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatus",
                        principalColumn: "RequestStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDescription = table.Column<string>(maxLength: 4096, nullable: true),
                    LogDate = table.Column<DateTime>(nullable: false),
                    RequestStatusId = table.Column<long>(nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_RequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatus",
                        principalColumn: "RequestStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "RequestStatus",
                columns: new[] { "RequestStatusId", "Description" },
                values: new object[,]
                {
                    { 1L, "New Request" },
                    { 2L, "Rejected by Manager" },
                    { 3L, "Approved by Manager" },
                    { 4L, "Rejected by Finance" },
                    { 5L, "Approved by Finance" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_RequestId",
                table: "Log",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_RequestStatusId",
                table: "Log",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_RequestStatusId",
                table: "Request",
                column: "RequestStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "RequestStatus");
        }
    }
}
