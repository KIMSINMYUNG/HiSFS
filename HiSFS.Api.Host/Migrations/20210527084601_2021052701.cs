using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021052701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<decimal>(
            //    name: "실제수량",
            //    table: "보유품목정보",
            //    type: "decimal(17,6)",
            //    nullable: false,
            //    defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "외주생산위치정보",
                columns: table => new
                {
                    보유품목위치순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    장소위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    위치상세코드 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사유 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    지시서 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    기타 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_외주생산위치정보", x => x.보유품목위치순번);
                });

            migrationBuilder.CreateIndex(
                name: "IX_외주생산위치정보_장소위치코드",
                table: "외주생산위치정보",
                column: "장소위치코드");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "실제수량",
                table: "보유품목정보");
        }
    }
}
