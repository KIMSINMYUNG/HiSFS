using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060303 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_공정이력정보_보유품목정보_회사코드_설비코드",
            //    table: "공정이력정보");

            //migrationBuilder.DropIndex(
            //    name: "IX_공정이력정보_회사코드_설비코드",
            //    table: "공정이력정보");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_회사코드_설비코드",
                table: "공정이력정보",
                columns: new[] { "회사코드", "설비코드" });

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력정보_보유품목정보_회사코드_설비코드",
                table: "공정이력정보",
                columns: new[] { "회사코드", "설비코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
