using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "입고유무",
                table: "바코드발급정보",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "입고일자",
                table: "바코드발급정보",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "작업일",
                table: "공정이력정보",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "입고유무",
                table: "바코드발급정보");

            migrationBuilder.DropColumn(
                name: "입고일자",
                table: "바코드발급정보");

            migrationBuilder.DropColumn(
                name: "작업일",
                table: "공정이력정보");
        }
    }
}
