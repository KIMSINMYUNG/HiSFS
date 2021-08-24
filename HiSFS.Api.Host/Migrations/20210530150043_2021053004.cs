using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "외주품질검사측정정보",
                columns: table => new
                {
                    시리얼넘버 = table.Column<int>(type: "int", nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    지시번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    검사단위코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산품공정명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    검사기준값 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    오차범위 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    검사측정값 = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    합격여부 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_외주품질검사측정정보", x => new { x.시리얼넘버, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_외주품질검사측정정보_공통코드_검사단위코드",
                        column: x => x.검사단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_외주품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                        columns: x => new { x.회사코드, x.보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_외주품질검사측정정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_외주품질검사측정정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_검사단위코드",
                table: "외주품질검사측정정보",
                column: "검사단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_품질검사코드",
                table: "외주품질검사측정정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_회사코드_보유품목코드",
                table: "외주품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "외주품질검사측정정보");
        }
    }
}
