using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_공정단위정보_공정단위코드",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_사업장_회사코드",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_생산지시정보_생산지시코드",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_생산품공정정보_생산품공정코드",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_직원정보_회사코드_작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropForeignKey(
                name: "FK_공정이력상세정보_품목정보_생산품코드",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_공정단위코드",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_생산지시코드",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_생산품공정코드",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_생산품코드",
                table: "공정이력상세정보");

            migrationBuilder.DropIndex(
                name: "IX_공정이력상세정보_회사코드_작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "공정단위코드",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "공정차수",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "목표수량",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "생산지시명",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "생산지시코드",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "생산품공정코드",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "생산품코드",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "작업일",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "작업자사번",
                table: "공정이력상세정보");

            migrationBuilder.DropColumn(
                name: "회사코드",
                table: "공정이력상세정보");

            migrationBuilder.RenameColumn(
                name: "종료일",
                table: "공정이력상세정보",
                newName: "종료타임");

            migrationBuilder.RenameColumn(
                name: "시작일",
                table: "공정이력상세정보",
                newName: "시작타임");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "종료타임",
                table: "공정이력상세정보",
                newName: "종료일");

            migrationBuilder.RenameColumn(
                name: "시작타임",
                table: "공정이력상세정보",
                newName: "시작일");

            migrationBuilder.AddColumn<string>(
                name: "공정단위코드",
                table: "공정이력상세정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "공정차수",
                table: "공정이력상세정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "목표수량",
                table: "공정이력상세정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "생산지시명",
                table: "공정이력상세정보",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "생산지시코드",
                table: "공정이력상세정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "생산품공정코드",
                table: "공정이력상세정보",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "생산품코드",
                table: "공정이력상세정보",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "작업일",
                table: "공정이력상세정보",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

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
                name: "IX_공정이력상세정보_공정단위코드",
                table: "공정이력상세정보",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력상세정보_생산지시코드",
                table: "공정이력상세정보",
                column: "생산지시코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력상세정보_생산품공정코드",
                table: "공정이력상세정보",
                column: "생산품공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력상세정보_생산품코드",
                table: "공정이력상세정보",
                column: "생산품코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력상세정보_회사코드_작업자사번",
                table: "공정이력상세정보",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_공정단위정보_공정단위코드",
                table: "공정이력상세정보",
                column: "공정단위코드",
                principalTable: "공정단위정보",
                principalColumn: "공정단위코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_사업장_회사코드",
                table: "공정이력상세정보",
                column: "회사코드",
                principalTable: "사업장",
                principalColumn: "회사코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_생산지시정보_생산지시코드",
                table: "공정이력상세정보",
                column: "생산지시코드",
                principalTable: "생산지시정보",
                principalColumn: "생산지시코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_생산품공정정보_생산품공정코드",
                table: "공정이력상세정보",
                column: "생산품공정코드",
                principalTable: "생산품공정정보",
                principalColumn: "생산품공정코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_직원정보_회사코드_작업자사번",
                table: "공정이력상세정보",
                columns: new[] { "회사코드", "작업자사번" },
                principalTable: "직원정보",
                principalColumns: new[] { "회사코드", "사번" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력상세정보_품목정보_생산품코드",
                table: "공정이력상세정보",
                column: "생산품코드",
                principalTable: "품목정보",
                principalColumn: "품목코드",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
