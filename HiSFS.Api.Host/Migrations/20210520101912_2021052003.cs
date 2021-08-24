using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021052003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "재고조정품목정보",
                columns: table => new
                {
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    원품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    관리차수 = table.Column<int>(type: "int", nullable: false),
                    품목명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    품목영문명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    품목구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    계정구분코드 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    조달분류 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    조달구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    재고단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    관리단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    환산계수 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    대분류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    대분류명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    중분류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    중분류명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    소분류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    소분류명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    품목유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    소재코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    규격종류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    규격 = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    단위코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOT여부 = table.Column<bool>(type: "bit", nullable: false),
                    LOT기본수량 = table.Column<int>(type: "int", nullable: false),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검사여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    거래처회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    거래처코드1 = table.Column<string>(type: "nvarchar(10)", nullable: true),
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
                    table.PrimaryKey("PK_재고조정품목정보", x => x.품목코드);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_거래처정보_거래처회사코드_거래처코드1",
                        columns: x => new { x.거래처회사코드, x.거래처코드1 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_규격종류코드",
                        column: x => x.규격종류코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_단위코드",
                        column: x => x.단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_소재코드",
                        column: x => x.소재코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_조달구분코드",
                        column: x => x.조달구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_품목구분코드",
                        column: x => x.품목구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_공통코드_품목유형코드",
                        column: x => x.품목유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_재고조정품목정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_거래처코드",
                table: "재고조정품목정보",
                column: "거래처코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_거래처회사코드_거래처코드1",
                table: "재고조정품목정보",
                columns: new[] { "거래처회사코드", "거래처코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_규격종류코드",
                table: "재고조정품목정보",
                column: "규격종류코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_단위코드",
                table: "재고조정품목정보",
                column: "단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_소재코드",
                table: "재고조정품목정보",
                column: "소재코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_원품목코드_관리차수",
                table: "재고조정품목정보",
                columns: new[] { "원품목코드", "관리차수" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_조달구분코드",
                table: "재고조정품목정보",
                column: "조달구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_품목구분코드",
                table: "재고조정품목정보",
                column: "품목구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_품목유형코드",
                table: "재고조정품목정보",
                column: "품목유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_재고조정품목정보_회사코드",
                table: "재고조정품목정보",
                column: "회사코드");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "재고조정품목정보");
        }
    }
}
