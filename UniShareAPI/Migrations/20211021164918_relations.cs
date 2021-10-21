using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniShareAPI.Migrations
{
    public partial class relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Since = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.FromId, x.ToId });
                    table.ForeignKey(
                        name: "FK_Relations_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relations_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relations_ToId",
                table: "Relations",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relations");
        }
    }
}
