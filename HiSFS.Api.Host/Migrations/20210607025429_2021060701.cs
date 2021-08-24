using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "실적공정코드",
                table: "생산실적헤더정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "실적작업장코드",
                table: "생산실적헤더정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "사용공정_사용창고",
                table: "생산실적상세정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "사용작업장_사용장소",
                table: "생산실적상세정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "실적공정코드",
                table: "생산실적헤더정보");

            migrationBuilder.DropColumn(
                name: "실적작업장코드",
                table: "생산실적헤더정보");

            migrationBuilder.DropColumn(
                name: "사용공정_사용창고",
                table: "생산실적상세정보");

            migrationBuilder.DropColumn(
                name: "사용작업장_사용장소",
                table: "생산실적상세정보");
        }
    }
}
