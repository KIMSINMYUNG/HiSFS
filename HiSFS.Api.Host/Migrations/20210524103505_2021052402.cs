using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021052402 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "공정단위코드",
                table: "외주작업지시서정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "생산품코드",
                table: "생산계획정보",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "공정단위코드",
                table: "외주작업지시서정보");

            migrationBuilder.AlterColumn<string>(
                name: "생산품코드",
                table: "생산계획정보",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
