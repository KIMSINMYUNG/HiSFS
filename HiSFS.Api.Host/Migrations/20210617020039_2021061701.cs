using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021061701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "작업자사번",
                table: "공정이력상세정보",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "회사코드",
                table: "공정이력상세정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_공정이력상세정보_회사코드_작업자사번",
                table: "공정이력상세정보",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_사업장_회사코드",
                table: "공정이력상세정보",
                column: "회사코드",
                principalTable: "사업장",
                principalColumn: "회사코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_직원정보_회사코드_작업자사번",
                table: "공정이력상세정보",
                columns: new[] { "회사코드", "작업자사번" },
                principalTable: "직원정보",
                principalColumns: new[] { "회사코드", "사번" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_사업장_회사코드",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_직원정보_회사코드_작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_회사코드_작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "회사코드",
                table: "공정이력상세정보");
        }
    }
}
