using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "주문번호",
                table: "생산실적상세정보",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "주문순번",
                table: "생산실적상세정보",
                type: "decimal(5,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "주문번호",
                table: "생산실적상세정보");

            migrationBuilder.DropColumn(
                name: "주문순번",
                table: "생산실적상세정보");
        }
    }
}
