using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보");

            migrationBuilder.AddColumn<decimal>(
                name: "발주순번",
                table: "발주서별품질검사측정정보",
                type: "decimal(5,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드", "발주번호", "발주순번" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "발주순번",
                table: "발주서별품질검사측정정보");

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드", "발주번호" });
        }
    }
}
