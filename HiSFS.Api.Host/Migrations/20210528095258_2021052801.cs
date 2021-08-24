using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021052801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "부서코드",
                table: "생산실적상세정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "공정품코드",
                table: "공정단위정보",
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
                name: "부서코드",
                table: "생산실적상세정보");

            migrationBuilder.AlterColumn<string>(
                name: "공정품코드",
                table: "공정단위정보",
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
