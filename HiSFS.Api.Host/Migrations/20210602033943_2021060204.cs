using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060204 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사정보",
                table: "발주서별품질검사정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사장비",
                table: "발주서별품질검사장비");

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사정보",
                table: "발주서별품질검사정보",
                columns: new[] { "회사코드", "발주번호", "발주순번", "품질검사코드" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사장비",
                table: "발주서별품질검사장비",
                columns: new[] { "발주번호", "발주순번", "검사장비식별번호" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사정보",
                table: "발주서별품질검사정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사장비",
                table: "발주서별품질검사장비");

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사정보",
                table: "발주서별품질검사정보",
                columns: new[] { "회사코드", "발주번호", "품질검사코드" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사장비",
                table: "발주서별품질검사장비",
                columns: new[] { "발주번호", "검사장비식별번호" });
        }
    }
}
