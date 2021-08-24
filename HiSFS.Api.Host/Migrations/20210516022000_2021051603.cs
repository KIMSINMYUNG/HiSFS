using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021051603 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "실적공정코드_창고코드",
                table: "작업자생산실적정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "실적작업장코드_장소코드",
                table: "작업자생산실적정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "실적공정코드_창고코드",
                table: "작업자생산실적정보");

            migrationBuilder.DropColumn(
                name: "실적작업장코드_장소코드",
                table: "작업자생산실적정보");
        }
    }
}
