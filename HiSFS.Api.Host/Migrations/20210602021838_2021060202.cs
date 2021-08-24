using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_발주서별품질검사측정정보_사업장_회사코드",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_발주서별품질검사측정정보_회사코드",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사");

            migrationBuilder.DropIndex(
                name: "IX_발주서별수입검사_회사코드",
                table: "발주서별수입검사");

            migrationBuilder.AlterColumn<string>(
                name: "회사코드",
                table: "발주서별품질검사측정정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보",
                columns: new[] { "회사코드", "시리얼넘버", "품질검사코드", "발주번호", "발주순번" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사",
                columns: new[] { "회사코드", "발주서번호", "발주번호", "발주순번" });

            migrationBuilder.AddForeignKey(
                name: "FK_발주서별품질검사측정정보_사업장_회사코드",
                table: "발주서별품질검사측정정보",
                column: "회사코드",
                principalTable: "사업장",
                principalColumn: "회사코드",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_발주서별품질검사측정정보_사업장_회사코드",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사");

            migrationBuilder.AlterColumn<string>(
                name: "회사코드",
                table: "발주서별품질검사측정정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별품질검사측정정보",
                table: "발주서별품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드", "발주번호", "발주순번" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사",
                columns: new[] { "발주서번호", "발주번호", "발주순번" });

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사측정정보_회사코드",
                table: "발주서별품질검사측정정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별수입검사_회사코드",
                table: "발주서별수입검사",
                column: "회사코드");

            migrationBuilder.AddForeignKey(
                name: "FK_발주서별품질검사측정정보_사업장_회사코드",
                table: "발주서별품질검사측정정보",
                column: "회사코드",
                principalTable: "사업장",
                principalColumn: "회사코드",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
