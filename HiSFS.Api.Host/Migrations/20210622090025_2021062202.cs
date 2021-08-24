using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021062202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "회사코드",
                table: "직원권한정보",
                type: "nvarchar(4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_직원권한정보_회사코드",
                table: "직원권한정보",
                column: "회사코드");

            migrationBuilder.AddForeignKey(
                name: "FK_직원권한정보_사업장_회사코드",
                table: "직원권한정보",
                column: "회사코드",
                principalTable: "사업장",
                principalColumn: "회사코드",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_직원권한정보_사업장_회사코드",
                table: "직원권한정보");

            migrationBuilder.DropIndex(
                name: "IX_직원권한정보_회사코드",
                table: "직원권한정보");

            migrationBuilder.AlterColumn<string>(
                name: "회사코드",
                table: "직원권한정보",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldNullable: true);
        }
    }
}
