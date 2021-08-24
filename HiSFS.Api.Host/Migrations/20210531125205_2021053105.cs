using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사");

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사",
                columns: new[] { "발주서번호", "발주번호", "발주순번" });

            migrationBuilder.CreateTable(
                name: "발주서별품질검사정보",
                columns: table => new
                {
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    발주순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    검사단위코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    검사기준값 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    오차범위 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    검사측정값 = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    합격여부 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    오차범위상한 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    오차범위하한 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_발주서별품질검사정보", x => new { x.발주번호, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_발주서별품질검사정보_공통코드_검사단위코드",
                        column: x => x.검사단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "발주서별품질검사측정정보",
                columns: table => new
                {
                    시리얼넘버 = table.Column<int>(type: "int", nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    발주번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_발주서별품질검사측정정보", x => new { x.시리얼넘버, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_발주서별품질검사측정정보_공통코드_검사단위코드",
                        column: x => x.검사단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                        columns: x => new { x.회사코드, x.보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사측정정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사측정정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "발주서별품질검사장비",
                columns: table => new
                {
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    검사장비식별번호 = table.Column<int>(type: "int", nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    발주순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_발주서별품질검사장비", x => new { x.발주번호, x.검사장비식별번호 });
                    table.ForeignKey(
                        name: "FK_발주서별품질검사장비_발주서별품질검사정보_발주번호_품질검사코드",
                        columns: x => new { x.발주번호, x.품질검사코드 },
                        principalTable: "발주서별품질검사정보",
                        principalColumns: new[] { "발주번호", "품질검사코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사장비_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주서별품질검사장비_연동장비정보_검사장비식별번호",
                        column: x => x.검사장비식별번호,
                        principalTable: "연동장비정보",
                        principalColumn: "식별번호",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사장비_검사장비식별번호",
                table: "발주서별품질검사장비",
                column: "검사장비식별번호");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사장비_발주번호_품질검사코드",
                table: "발주서별품질검사장비",
                columns: new[] { "발주번호", "품질검사코드" });

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사장비_회사코드",
                table: "발주서별품질검사장비",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사정보_검사단위코드",
                table: "발주서별품질검사정보",
                column: "검사단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사정보_품질검사코드",
                table: "발주서별품질검사정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사정보_회사코드",
                table: "발주서별품질검사정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사측정정보_검사단위코드",
                table: "발주서별품질검사측정정보",
                column: "검사단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사측정정보_품질검사코드",
                table: "발주서별품질검사측정정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주서별품질검사측정정보_회사코드_보유품목코드",
                table: "발주서별품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "발주서별품질검사장비");

            migrationBuilder.DropTable(
                name: "발주서별품질검사측정정보");

            migrationBuilder.DropTable(
                name: "발주서별품질검사정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사");

            migrationBuilder.AddPrimaryKey(
                name: "PK_발주서별수입검사",
                table: "발주서별수입검사",
                columns: new[] { "발주서번호", "발주번호" });
        }
    }
}
