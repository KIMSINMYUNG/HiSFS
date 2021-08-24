using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "검사수량",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "공정단위코드",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "불량수량",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "생산수량",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "시작일",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "실생산량",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "실행상태코드",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "종료일",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "품질검사완료여부",
                table: "외주작업지시서정보");

            migrationBuilder.DropColumn(
                name: "합격수량",
                table: "외주작업지시서정보");

            migrationBuilder.CreateTable(
                name: "외주작업지시서품검정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    지시번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    전개순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    지시일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    완료일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    관리단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    공정 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    공정명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    작업장 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    작업장명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    외주단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    외주금액 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    설비코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    설비명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    비고_DOC_DC = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    지시상태 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    지시상태명 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    지시구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    지시구분명 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    생산외주구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    생산외주구분명 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    처리구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    처리구분명 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    검사구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    검사구분명 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래처약칭 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    주문번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    주문순번 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    작업팀 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    작업팀명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    작업조 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    작업조명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품질검사완료여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실행상태코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    실생산량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_외주작업지시서품검정보", x => new { x.회사코드, x.지시번호, x.전개순번 });
                    table.ForeignKey(
                        name: "FK_외주작업지시서품검정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "외주작업지시서품검정보");

            migrationBuilder.AddColumn<decimal>(
                name: "검사수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "공정단위코드",
                table: "외주작업지시서정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "불량수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "생산수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "시작일",
                table: "외주작업지시서정보",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "실생산량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "실행상태코드",
                table: "외주작업지시서정보",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "종료일",
                table: "외주작업지시서정보",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "품질검사완료여부",
                table: "외주작업지시서정보",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "합격수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
