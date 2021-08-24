using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "발주서별수입검사",
                columns: table => new
                {
                    발주서번호 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    검사구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    발주순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    관리단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    납기일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    출하예정일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    발주수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    발주단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    공급가 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    부가세 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    합계액 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    관리구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    관리구분명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    프로젝트 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    프록젝트명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    비고_내역 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    환종 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    부가세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    발주완료유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    수입검사완료유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실행상태코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    총수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    검사수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    합격수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    시작일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    종료일 = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_발주서별수입검사", x => new { x.발주서번호, x.발주번호 });
                    table.ForeignKey(
                        name: "FK_발주서별수입검사_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_발주서별수입검사_회사코드",
                table: "발주서별수입검사",
                column: "회사코드");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "발주서별수입검사");
        }
    }
}
