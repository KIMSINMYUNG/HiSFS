using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목회사코드",
                table: "품질검사측정정보");

            migrationBuilder.AddColumn<string>(
                name: "입고여부",
                table: "발주서별수입검사",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "입고여부",
                table: "발주서별수입검사");

            migrationBuilder.AddColumn<string>(
                name: "보유품목코드1",
                table: "품질검사측정정보",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "보유품목회사코드",
                table: "품질검사측정정보",
                type: "nvarchar(4)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" });

            migrationBuilder.AddForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
