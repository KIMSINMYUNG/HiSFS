using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021052401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "주문번호",
                table: "생산계획정보",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "주문번호",
                table: "생산계획정보");
        }
    }
}
