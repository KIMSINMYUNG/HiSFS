using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060205 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "공정불량정보",
                columns: table => new
                {
                    인덱스 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    생산지시명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    설비코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    작업자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    불량유형 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    공정차수 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    작업일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_공정불량정보", x => x.인덱스);
                    table.ForeignKey(
                        name: "FK_공정불량정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_공통코드_불량유형",
                        column: x => x.불량유형,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_보유품목정보_회사코드_설비코드",
                        columns: x => new { x.회사코드, x.설비코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_생산지시정보_생산지시코드",
                        column: x => x.생산지시코드,
                        principalTable: "생산지시정보",
                        principalColumn: "생산지시코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_생산품공정정보_생산품공정코드",
                        column: x => x.생산품공정코드,
                        principalTable: "생산품공정정보",
                        principalColumn: "생산품공정코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_직원정보_회사코드_작업자사번",
                        columns: x => new { x.회사코드, x.작업자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정불량정보_품목정보_생산품코드",
                        column: x => x.생산품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_공정단위코드",
                table: "공정불량정보",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_불량유형",
                table: "공정불량정보",
                column: "불량유형");

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_생산지시코드",
                table: "공정불량정보",
                column: "생산지시코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_생산품공정코드",
                table: "공정불량정보",
                column: "생산품공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_생산품코드",
                table: "공정불량정보",
                column: "생산품코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_회사코드_설비코드",
                table: "공정불량정보",
                columns: new[] { "회사코드", "설비코드" });

            migrationBuilder.CreateIndex(
                name: "IX_공정불량정보_회사코드_작업자사번",
                table: "공정불량정보",
                columns: new[] { "회사코드", "작업자사번" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "공정불량정보");
        }
    }
}
