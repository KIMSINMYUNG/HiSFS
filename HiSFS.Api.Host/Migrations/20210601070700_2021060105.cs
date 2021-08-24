using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "총수량",
                table: "발주서별수입검사");

            migrationBuilder.AddColumn<string>(
                name: "품질검사완료여부",
                table: "발주서별수입검사",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "품질검사완료여부",
                table: "발주서별수입검사");

            migrationBuilder.AddColumn<decimal>(
                name: "총수량",
                table: "발주서별수입검사",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
