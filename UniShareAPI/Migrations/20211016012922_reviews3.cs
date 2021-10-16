using Microsoft.EntityFrameworkCore.Migrations;

namespace UniShareAPI.Migrations
{
    public partial class reviews3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Helpful",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Helpful",
                table: "Reviews");
        }
    }
}
