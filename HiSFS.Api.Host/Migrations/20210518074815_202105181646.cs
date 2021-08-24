using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _202105181646 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "기타",
                table: "보유품목임시위치정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "지시서",
                table: "보유품목임시위치정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "기타",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "지시서",
                table: "보유품목임시위치정보");
        }
    }
}
