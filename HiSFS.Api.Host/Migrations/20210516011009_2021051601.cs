using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021051601 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "공통코드",
                columns: table => new
                {
                    코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    상위코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    코드명 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    코드영문명 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    설명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    인자1 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    인자2 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    인자3 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    코드유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    뎁스 = table.Column<int>(type: "int", nullable: false),
                    정렬순번 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_공통코드", x => x.코드);
                    table.ForeignKey(
                        name: "FK_공통코드_공통코드_상위코드",
                        column: x => x.상위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공통코드_공통코드_코드유형코드",
                        column: x => x.코드유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "메뉴정보",
                columns: table => new
                {
                    순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    메뉴명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    경로명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    클래스명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    상위메뉴순번 = table.Column<int>(type: "int", nullable: true),
                    뎁스 = table.Column<int>(type: "int", nullable: false),
                    정렬순번 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_메뉴정보", x => x.순번);
                    table.ForeignKey(
                        name: "FK_메뉴정보_메뉴정보_상위메뉴순번",
                        column: x => x.상위메뉴순번,
                        principalTable: "메뉴정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "물류담당자정보",
                columns: table => new
                {
                    물류담당자번호 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    사원명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    전화번호 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    팩스번호 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    핸드폰번호 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    담당그룹코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    담당그룹명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    시작일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    종료일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_물류담당자정보", x => x.물류담당자번호);
                });

            migrationBuilder.CreateTable(
                name: "발주서헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    발주일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_발주서헤더정보", x => new { x.회사코드, x.발주번호 });
                });

            migrationBuilder.CreateTable(
                name: "보유품목임시위치정보",
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
                    사유 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_보유품목임시위치정보", x => x.보유품목위치순번);
                });

            migrationBuilder.CreateTable(
                name: "사업장",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사업장명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_사업장", x => x.회사코드);
                });

            migrationBuilder.CreateTable(
                name: "사용자재보고정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업순번 = table.Column<decimal>(type: "decimal(3,0)", nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    사용공정 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사용작업장 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    지시번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    지시구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    유무상구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    유효여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실적번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    사용순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    사용일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    사용수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    지시전개순번 = table.Column<decimal>(type: "decimal(3,0)", nullable: false),
                    소요자재순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    관리내역코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    비고_보조언어 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    최초입력사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    최초입력일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    최초입력IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    수정사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    수정일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    수정IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    외부연동작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PDA번호 = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
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
                    table.PrimaryKey("PK_사용자재보고정보", x => new { x.회사코드, x.작업번호, x.작업순번 });
                });

            migrationBuilder.CreateTable(
                name: "외주작업지시헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    지시번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    전개순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    공정 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    공정명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    지시구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    지시구분명 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    생산외주구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    생산외주구분명 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
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
                    table.PrimaryKey("PK_외주작업지시헤더정보", x => new { x.회사코드, x.지시번호 });
                });

            migrationBuilder.CreateTable(
                name: "일괄생산실적헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    실적번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    실적일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    실적공정코드_창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실적작업장코드_장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    재작업여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실적품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    실적수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOTNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    관리구분 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    설비코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    실적구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실적공정코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실적작업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    작업자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
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
                    table.PrimaryKey("PK_일괄생산실적헤더정보", x => new { x.회사코드, x.작업번호 });
                });

            migrationBuilder.CreateTable(
                name: "입고처리헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    입고구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    입고번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    입고일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    입고창고 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    입고장소 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    거래구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    환종 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    환율 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LC여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    작업구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    관리구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EXCST_NB = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    배부여부 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    최초입력사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    최초입력일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    최초입력IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    수정사원코드 = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: true),
                    수정일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    수정IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DUMMY1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DUMMY2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DUMMY3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PLN_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SO_NB3 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    UMVAT_FG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    APP_FG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_입고처리헤더정보", x => new { x.회사코드, x.작업번호 });
                });

            migrationBuilder.CreateTable(
                name: "작업외주생산실적등록정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    실적번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    실적일자 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    지시번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    지시전개순번 = table.Column<decimal>(type: "decimal(3,0)", nullable: false),
                    실적수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    실적공정코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실적작업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    처리구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    이동공정_입고창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    이동작업장_입고장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    검사구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    실적구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    불량유형코드 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    재작업여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    작업자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    설비코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    작업팀코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    작업조코드 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    부산물여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    부산물품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    주산물원천실적번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    최초입력사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    최초입력일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    최초입력IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    수정사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    수정일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    수정IP = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PDA아이디 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PDA번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    지시구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    지시구분명 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_작업외주생산실적등록정보", x => new { x.회사코드, x.작업번호 });
                });

            migrationBuilder.CreateTable(
                name: "재고이동헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    이동번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    이동일자 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    이동구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    입출고구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    부서코드 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    출고창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    출고장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    출고장소위치상세코드 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    입고공정_창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    입고작업장_장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    입고장소위치상세코드 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    헤더비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    처리구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    APP_FG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_재고이동헤더정보", x => new { x.회사코드, x.작업번호 });
                });

            migrationBuilder.CreateTable(
                name: "재고조정정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    조정번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    조정순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    조정구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    조정구분명 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    조정일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    창고명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    장소명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    관리단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    환산계수 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    조정수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    조정금액 = table.Column<decimal>(type: "decimal(17,4)", nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    관리구분 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    관리구분명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    프로젝트명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    비고_건 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    비고_내역 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래처 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "재고조정정보이력",
                columns: table => new
                {
                    CO_CD = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ADJUST_NB = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ADJUST_SQ = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    ADJUST_FG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ADJUST_FG_NM = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ADJUST_DT = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    WH_CD = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    WH_NM = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    LC_CD = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    LC_NM = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PLN_CD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PLN_NM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ITEM_CD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ITEM_NM = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ITEM_DC = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UNIT_DC = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    UNITMANG_DC = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    UNITCHNG_NB = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    QT = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    ADJUST_UM = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    ADJUST_AM = table.Column<decimal>(type: "decimal(17,4)", nullable: false),
                    LOT_NB = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MGMT_CD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MGM_NM = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PJT_CD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PJT_NM = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    REMARK_DC_H = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    REMARK_DC_D = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    TR_CD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TR_NM = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_재고조정정보이력", x => new { x.CO_CD, x.ADJUST_NB, x.ADJUST_SQ });
                });

            migrationBuilder.CreateTable(
                name: "주문서헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    주문번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    주문일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    고객명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    주문구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    납품처명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_주문서헤더정보", x => new { x.회사코드, x.주문번호 });
                });

            migrationBuilder.CreateTable(
                name: "출고처리헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업일자 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    출고구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    출고일자 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    주문번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    거래구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    환종 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    환율 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    단가구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    연동구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    납품처코드 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
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
                    table.PrimaryKey("PK_출고처리헤더정보", x => new { x.회사코드, x.작업번호 });
                });

            migrationBuilder.CreateTable(
                name: "파일폴더정보",
                columns: table => new
                {
                    순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    폴더명 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    폴더경로 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
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
                    table.PrimaryKey("PK_파일폴더정보", x => x.순번);
                });

            migrationBuilder.CreateTable(
                name: "품질검사정보",
                columns: table => new
                {
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    품질검사명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_품질검사정보", x => x.품질검사코드);
                });

            migrationBuilder.CreateTable(
                name: "BOM_정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    모품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    모품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    모규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    모품목재고단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    자품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    자품명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    자규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    자품목재고단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    정미수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOSS율 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    필요수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    외주구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    임가공구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    주거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    주거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    시작일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    종료일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_BOM_정보", x => new { x.회사코드, x.모품번, x.순번 });
                });

            migrationBuilder.CreateTable(
                name: "BOM품목정보",
                columns: table => new
                {
                    BOM품목정보코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    품목구분코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    정미수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    로스율 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    필요수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_BOM품목정보", x => x.BOM품목정보코드);
                });

            migrationBuilder.CreateTable(
                name: "거래처정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    거래처명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    거래처약칭 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    거래처구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    거래처구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    등록번호 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    담당자 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    업태 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    종목 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    주소 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    주소2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    대표연락처 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    담당자연락처 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    공급가격 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    팩스및비고 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    이메일 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    화물도착지 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    거래1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    거래2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    거래3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    거래4 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    거래5 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_거래처정보", x => new { x.회사코드, x.거래처코드 });
                    table.ForeignKey(
                        name: "FK_거래처정보_공통코드_거래처구분코드",
                        column: x => x.거래처구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "공정정보",
                columns: table => new
                {
                    공정코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    공정명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    공정유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    설비사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    설비유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_공정정보", x => x.공정코드);
                    table.ForeignKey(
                        name: "FK_공정정보_공통코드_공정유형코드",
                        column: x => x.공정유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정정보_공통코드_설비유형코드",
                        column: x => x.설비유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "액션정보",
                columns: table => new
                {
                    액션코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    액션명 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    액션유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    액션인자 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    액션인자설명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    변경액션유무 = table.Column<bool>(type: "bit", nullable: false),
                    대체액션코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_액션정보", x => x.액션코드);
                    table.ForeignKey(
                        name: "FK_액션정보_공통코드_액션유형코드",
                        column: x => x.액션유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_액션정보_액션정보_대체액션코드",
                        column: x => x.대체액션코드,
                        principalTable: "액션정보",
                        principalColumn: "액션코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "연동장비정보",
                columns: table => new
                {
                    식별번호 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    식별코드 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    장비명 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    에이전트명 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    연동장비유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    테스트 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    등록시각 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    승인시각 = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_연동장비정보", x => x.식별번호);
                    table.ForeignKey(
                        name: "FK_연동장비정보_공통코드_연동장비유형코드",
                        column: x => x.연동장비유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "메뉴유형별권한정보",
                columns: table => new
                {
                    메뉴순번 = table.Column<int>(type: "int", nullable: false),
                    권한유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    읽기권한 = table.Column<bool>(type: "bit", nullable: true),
                    등록권한 = table.Column<bool>(type: "bit", nullable: true),
                    변경권한 = table.Column<bool>(type: "bit", nullable: true),
                    삭제권한 = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_메뉴유형별권한정보", x => new { x.메뉴순번, x.권한유형코드 });
                    table.ForeignKey(
                        name: "FK_메뉴유형별권한정보_공통코드_권한유형코드",
                        column: x => x.권한유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_메뉴유형별권한정보_메뉴정보_메뉴순번",
                        column: x => x.메뉴순번,
                        principalTable: "메뉴정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "발주서정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    발주순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    발주서번호 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_발주서정보", x => new { x.회사코드, x.발주번호, x.발주순번 });
                    table.ForeignKey(
                        name: "FK_발주서정보_발주서헤더정보_회사코드_발주번호",
                        columns: x => new { x.회사코드, x.발주번호 },
                        principalTable: "발주서헤더정보",
                        principalColumns: new[] { "회사코드", "발주번호" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_발주서정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "부서정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    부서코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    부서명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    정렬순번 = table.Column<int>(type: "int", nullable: false),
                    선택가능유무 = table.Column<bool>(type: "bit", nullable: false),
                    연계부서코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    부문코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    부문명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_부서정보", x => new { x.회사코드, x.부서코드 });
                    table.ForeignKey(
                        name: "FK_부서정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "장소정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    장소코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    장소명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    장소유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    공정구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    공정구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    장소사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_장소정보", x => new { x.회사코드, x.장소코드 });
                    table.ForeignKey(
                        name: "FK_장소정보_공통코드_공정구분코드",
                        column: x => x.공정구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_장소정보_공통코드_장소유형코드",
                        column: x => x.장소유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_장소정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "직원권한정보",
                columns: table => new
                {
                    식별인자 = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    암호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    암호암호화유무 = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_직원권한정보", x => x.식별인자);
                    table.ForeignKey(
                        name: "FK_직원권한정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "외주작업지시서정보",
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
                    table.PrimaryKey("PK_외주작업지시서정보", x => new { x.회사코드, x.지시번호, x.전개순번 });
                    table.ForeignKey(
                        name: "FK_외주작업지시서정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_외주작업지시서정보_외주작업지시헤더정보_회사코드_지시번호",
                        columns: x => new { x.회사코드, x.지시번호 },
                        principalTable: "외주작업지시헤더정보",
                        principalColumns: new[] { "회사코드", "지시번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "일괄생산실적상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업순번 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    실적번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    실적순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    사용공정_사용창고 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사용작업장_사용장소 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사용품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    사용수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOTNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    창고구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_일괄생산실적상세정보", x => new { x.회사코드, x.작업번호, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_일괄생산실적상세정보_일괄생산실적헤더정보_회사코드_작업번호",
                        columns: x => new { x.회사코드, x.작업번호 },
                        principalTable: "일괄생산실적헤더정보",
                        principalColumns: new[] { "회사코드", "작업번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "입고처리상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    입고번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    입고순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    입고수량_관리단위 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    입고수량_재고단위 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    입고단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    공급가 = table.Column<decimal>(type: "decimal(17,4)", nullable: true),
                    부가세 = table.Column<decimal>(type: "decimal(17,4)", nullable: true),
                    합계액 = table.Column<decimal>(type: "decimal(17,4)", nullable: true),
                    환종 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    환율 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    외화단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: true),
                    외화금액 = table.Column<decimal>(type: "decimal(17,4)", nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    발주번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    발주순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    입고의뢰번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    입고의뢰순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    선적번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    선적순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: true),
                    사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    유효여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    단가구분 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    입고장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_입고처리상세정보", x => new { x.회사코드, x.작업번호, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_입고처리상세정보_입고처리헤더정보_회사코드_작업번호",
                        columns: x => new { x.회사코드, x.작업번호 },
                        principalTable: "입고처리헤더정보",
                        principalColumns: new[] { "회사코드", "작업번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "재고이동상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    이동번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    이동순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    청구수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    이동수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    재공운영여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    모품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    관리구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    지시번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    청구순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    APP_FG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    상세_비고 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    사용여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    만료여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_재고이동상세정보", x => new { x.회사코드, x.작업번호, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_재고이동상세정보_재고이동헤더정보_회사코드_작업번호",
                        columns: x => new { x.회사코드, x.작업번호 },
                        principalTable: "재고이동헤더정보",
                        principalColumns: new[] { "회사코드", "작업번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "주문서정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    주문번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    주문서번호 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    주문일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    고객코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    고객명 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    주문구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    과세구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    단가구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    단가구분명 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    납품처코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    납품처명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    담당자코드 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    담당자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    관리번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    헤더비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품목명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    규격 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    관리단위 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    납기일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    출하예정일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    부가세단가 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    공급가 = table.Column<decimal>(type: "decimal(17,4)", nullable: false),
                    SOV_AM = table.Column<decimal>(type: "decimal(17,4)", nullable: false),
                    합계액 = table.Column<decimal>(type: "decimal(17,4)", nullable: false),
                    관리구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    관리구분명 = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    프로젝트코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    프로젝트명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    디테일비고 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    마감여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    검사구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    환종 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    주문완료유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_주문서정보", x => new { x.회사코드, x.주문번호, x.순번 });
                    table.ForeignKey(
                        name: "FK_주문서정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_주문서정보_주문서헤더정보_회사코드_주문번호",
                        columns: x => new { x.회사코드, x.주문번호 },
                        principalTable: "주문서헤더정보",
                        principalColumns: new[] { "회사코드", "주문번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "출고처리상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    작업순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    출고수량_관리단위 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    출고수량_재고단위 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    주문번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    주문순번 = table.Column<decimal>(type: "decimal(5,0)", nullable: false),
                    장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    연동구분 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_출고처리상세정보", x => new { x.회사코드, x.작업번호, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_출고처리상세정보_출고처리헤더정보_회사코드_작업번호",
                        columns: x => new { x.회사코드, x.작업번호 },
                        principalTable: "출고처리헤더정보",
                        principalColumns: new[] { "회사코드", "작업번호" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "도면정보",
                columns: table => new
                {
                    도면코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    원도면코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    관리차수 = table.Column<int>(type: "int", nullable: false),
                    도면번호 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    도면명 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    도면영문명 = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    도면종류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    개요 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    설명 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    파일폴더순번 = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_도면정보", x => x.도면코드);
                    table.ForeignKey(
                        name: "FK_도면정보_공통코드_도면종류코드",
                        column: x => x.도면종류코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_도면정보_파일폴더정보_파일폴더순번",
                        column: x => x.파일폴더순번,
                        principalTable: "파일폴더정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "파일정보",
                columns: table => new
                {
                    순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    파일이름 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    설명 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    확장자 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    크기 = table.Column<int>(type: "int", nullable: false),
                    경로 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    임시경로 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    썸네일경로 = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    폴더순번 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_파일정보", x => x.순번);
                    table.ForeignKey(
                        name: "FK_파일정보_파일폴더정보_폴더순번",
                        column: x => x.폴더순번,
                        principalTable: "파일폴더정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "발주정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    발주순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    발주서명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주상태코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    입고예정일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_발주정보", x => new { x.발주순번, x.회사코드 });
                    table.ForeignKey(
                        name: "FK_발주정보_거래처정보_거래처회사코드_거래처코드1",
                        columns: x => new { x.거래처회사코드, x.거래처코드1 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "품목정보",
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
                    table.PrimaryKey("PK_품목정보", x => x.품목코드);
                    table.ForeignKey(
                        name: "FK_품목정보_거래처정보_거래처회사코드_거래처코드1",
                        columns: x => new { x.거래처회사코드, x.거래처코드1 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_규격종류코드",
                        column: x => x.규격종류코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_단위코드",
                        column: x => x.단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_소재코드",
                        column: x => x.소재코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_조달구분코드",
                        column: x => x.조달구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_품목구분코드",
                        column: x => x.품목구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_공통코드_품목유형코드",
                        column: x => x.품목유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품목정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "메뉴부서권한정보",
                columns: table => new
                {
                    메뉴순번 = table.Column<int>(type: "int", nullable: false),
                    부서코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    읽기권한 = table.Column<bool>(type: "bit", nullable: true),
                    등록권한 = table.Column<bool>(type: "bit", nullable: true),
                    변경권한 = table.Column<bool>(type: "bit", nullable: true),
                    삭제권한 = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_메뉴부서권한정보", x => new { x.메뉴순번, x.부서코드 });
                    table.ForeignKey(
                        name: "FK_메뉴부서권한정보_메뉴정보_메뉴순번",
                        column: x => x.메뉴순번,
                        principalTable: "메뉴정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_메뉴부서권한정보_부서정보_회사코드_부서코드",
                        columns: x => new { x.회사코드, x.부서코드 },
                        principalTable: "부서정보",
                        principalColumns: new[] { "회사코드", "부서코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_메뉴부서권한정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "장소위치정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    장소위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    장소코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    위치명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    위치분류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_장소위치정보", x => new { x.회사코드, x.장소위치코드 });
                    table.ForeignKey(
                        name: "FK_장소위치정보_공통코드_위치분류코드",
                        column: x => x.위치분류코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_장소위치정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_장소위치정보_장소정보_회사코드_장소코드",
                        columns: x => new { x.회사코드, x.장소코드 },
                        principalTable: "장소정보",
                        principalColumns: new[] { "회사코드", "장소코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "직원정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    사용자명 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    식별번호 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    식별인자 = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    부서코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    권한코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    입사일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    퇴사일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    직급코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    직책코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    주소 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    상세주소 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    연락처1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    연락처2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    이메일 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_직원정보", x => new { x.회사코드, x.사번 });
                    table.ForeignKey(
                        name: "FK_직원정보_공통코드_권한코드",
                        column: x => x.권한코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_직원정보_공통코드_직급코드",
                        column: x => x.직급코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_직원정보_공통코드_직책코드",
                        column: x => x.직책코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_직원정보_부서정보_회사코드_부서코드",
                        columns: x => new { x.회사코드, x.부서코드 },
                        principalTable: "부서정보",
                        principalColumns: new[] { "회사코드", "부서코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_직원정보_직원권한정보_식별인자",
                        column: x => x.식별인자,
                        principalTable: "직원권한정보",
                        principalColumn: "식별인자");
                });

            migrationBuilder.CreateTable(
                name: "공정단위정보",
                columns: table => new
                {
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    원공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    관리차수 = table.Column<int>(type: "int", nullable: false),
                    공정단위명 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    공정품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    공정품유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    도면코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    공정코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    완제품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    공정예상시간 = table.Column<double>(type: "float", nullable: false),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_공정단위정보", x => x.공정단위코드);
                    table.ForeignKey(
                        name: "FK_공정단위정보_공정정보_공정코드",
                        column: x => x.공정코드,
                        principalTable: "공정정보",
                        principalColumn: "공정코드");
                    table.ForeignKey(
                        name: "FK_공정단위정보_공통코드_공정품유형코드",
                        column: x => x.공정품유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드");
                    table.ForeignKey(
                        name: "FK_공정단위정보_도면정보_도면코드",
                        column: x => x.도면코드,
                        principalTable: "도면정보",
                        principalColumn: "도면코드");
                    table.ForeignKey(
                        name: "FK_공정단위정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정단위정보_품목정보_공정품코드",
                        column: x => x.공정품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드");
                    table.ForeignKey(
                        name: "FK_공정단위정보_품목정보_완제품코드",
                        column: x => x.완제품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "바코드발급정보",
                columns: table => new
                {
                    발급순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    사원코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    수량 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    생성일자 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_바코드발급정보", x => x.발급순번);
                    table.ForeignKey(
                        name: "FK_바코드발급정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_바코드발급정보_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "발주정보상세",
                columns: table => new
                {
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    발주순번 = table.Column<int>(type: "int", nullable: false),
                    발주상세순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    품목구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    입고수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    발주정보발주순번 = table.Column<int>(type: "int", nullable: true),
                    발주정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
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
                    table.PrimaryKey("PK_발주정보상세", x => new { x.발주순번, x.품목코드 });
                    table.ForeignKey(
                        name: "FK_발주정보상세_공통코드_품목구분코드",
                        column: x => x.품목구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주정보상세_발주정보_발주정보발주순번_발주정보회사코드",
                        columns: x => new { x.발주정보발주순번, x.발주정보회사코드 },
                        principalTable: "발주정보",
                        principalColumns: new[] { "발주순번", "회사코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_발주정보상세_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "생산품공정정보",
                columns: table => new
                {
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    생산품공정명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    생산품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    관리차수 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_생산품공정정보", x => x.생산품공정코드);
                    table.ForeignKey(
                        name: "FK_생산품공정정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산품공정정보_품목정보_생산품코드",
                        column: x => x.생산품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOM정보",
                columns: table => new
                {
                    BOM순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    상위BOM순번 = table.Column<int>(type: "int", nullable: true),
                    정미수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    로스율 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    필요수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_BOM정보", x => x.BOM순번);
                    table.ForeignKey(
                        name: "FK_BOM정보_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOM정보_BOM정보_상위BOM순번",
                        column: x => x.상위BOM순번,
                        principalTable: "BOM정보",
                        principalColumn: "BOM순번");
                });

            migrationBuilder.CreateTable(
                name: "위치상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    위치상세코드 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    장소위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    상세코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    위치명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    위치분류코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    위치상세분류코드 = table.Column<string>(type: "nvarchar(10)", nullable: true),
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
                    table.PrimaryKey("PK_위치상세정보", x => new { x.회사코드, x.위치상세코드 });
                    table.ForeignKey(
                        name: "FK_위치상세정보_공통코드_위치상세분류코드",
                        column: x => x.위치상세분류코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_위치상세정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_위치상세정보_장소위치정보_회사코드_장소위치코드",
                        columns: x => new { x.회사코드, x.장소위치코드 },
                        principalTable: "장소위치정보",
                        principalColumns: new[] { "회사코드", "장소위치코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "메뉴직원권한정보",
                columns: table => new
                {
                    메뉴순번 = table.Column<int>(type: "int", nullable: false),
                    직원사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    읽기권한 = table.Column<bool>(type: "bit", nullable: true),
                    등록권한 = table.Column<bool>(type: "bit", nullable: true),
                    변경권한 = table.Column<bool>(type: "bit", nullable: true),
                    삭제권한 = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_메뉴직원권한정보", x => new { x.메뉴순번, x.직원사번 });
                    table.ForeignKey(
                        name: "FK_메뉴직원권한정보_메뉴정보_메뉴순번",
                        column: x => x.메뉴순번,
                        principalTable: "메뉴정보",
                        principalColumn: "순번",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_메뉴직원권한정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_메뉴직원권한정보_직원정보_회사코드_직원사번",
                        columns: x => new { x.회사코드, x.직원사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "메시지정보",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    전체메시지유무 = table.Column<bool>(type: "bit", nullable: false),
                    발송인사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    수신인사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    메시지확인유무 = table.Column<bool>(type: "bit", nullable: false),
                    메시지명 = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    메시지 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    메시지유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_메시지정보", x => x.Id);
                    table.ForeignKey(
                        name: "FK_메시지정보_공통코드_메시지유형코드",
                        column: x => x.메시지유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_메시지정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_메시지정보_직원정보_회사코드_발송인사번",
                        columns: x => new { x.회사코드, x.발송인사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_메시지정보_직원정보_회사코드_수신인사번",
                        columns: x => new { x.회사코드, x.수신인사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "액션로그",
                columns: table => new
                {
                    순번 = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    직원사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    액션코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    액션인자 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    액션시각 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    연동장비식별번호 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_액션로그", x => x.순번);
                    table.ForeignKey(
                        name: "FK_액션로그_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_액션로그_액션정보_액션코드",
                        column: x => x.액션코드,
                        principalTable: "액션정보",
                        principalColumn: "액션코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_액션로그_연동장비정보_연동장비식별번호",
                        column: x => x.연동장비식별번호,
                        principalTable: "연동장비정보",
                        principalColumn: "식별번호",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_액션로그_직원정보_회사코드_직원사번",
                        columns: x => new { x.회사코드, x.직원사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "작업자생산실적정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    작업순번 = table.Column<int>(type: "int", nullable: false),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    작업자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    실적수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    사용품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    사용수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    실적등록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    일괄생산등록유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_작업자생산실적정보", x => new { x.회사코드, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_작업자생산실적정보_직원정보_회사코드_작업자사번",
                        columns: x => new { x.회사코드, x.작업자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "공정단위검사정보",
                columns: table => new
                {
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_공정단위검사정보", x => new { x.공정단위코드, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_공정단위검사정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정단위검사정보_공통코드_검사단위코드",
                        column: x => x.검사단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정단위검사정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정단위검사정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "공정단위자재정보",
                columns: table => new
                {
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    자재코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_공정단위자재정보", x => new { x.공정단위코드, x.자재코드 });
                    table.ForeignKey(
                        name: "FK_공정단위자재정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정단위자재정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정단위자재정보_품목정보_자재코드",
                        column: x => x.자재코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOM품목정보상세",
                columns: table => new
                {
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BOM품목정보코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    필요수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    레벨 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    공정단위정보공정단위코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
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
                    table.PrimaryKey("PK_BOM품목정보상세", x => new { x.품목코드, x.BOM품목정보코드 });
                    table.ForeignKey(
                        name: "FK_BOM품목정보상세_공정단위정보_공정단위정보공정단위코드",
                        column: x => x.공정단위정보공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BOM품목정보상세_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOM품목정보상세_BOM품목정보_BOM품목정보코드",
                        column: x => x.BOM품목정보코드,
                        principalTable: "BOM품목정보",
                        principalColumn: "BOM품목정보코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "생산실적헤더정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    생산품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    생산지시명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    사업장코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실적공정코드_창고코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실적작업장코드_장소코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    재작업여부 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LOTNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    일괄생산등록유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    작업번호 = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    실적수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_생산실적헤더정보", x => new { x.회사코드, x.생산지시코드 });
                    table.ForeignKey(
                        name: "FK_생산실적헤더정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드");
                    table.ForeignKey(
                        name: "FK_생산실적헤더정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산실적헤더정보_생산품공정정보_생산품공정코드",
                        column: x => x.생산품공정코드,
                        principalTable: "생산품공정정보",
                        principalColumn: "생산품공정코드");
                    table.ForeignKey(
                        name: "FK_생산실적헤더정보_품목정보_생산품코드",
                        column: x => x.생산품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드");
                });

            migrationBuilder.CreateTable(
                name: "생산품공정차수정보",
                columns: table => new
                {
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    공정차수 = table.Column<int>(type: "int", nullable: false),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    공정단위정보공정단위코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
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
                    table.PrimaryKey("PK_생산품공정차수정보", x => new { x.생산품공정코드, x.순번 });
                    table.ForeignKey(
                        name: "FK_생산품공정차수정보_공정단위정보_공정단위정보공정단위코드",
                        column: x => x.공정단위정보공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산품공정차수정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산품공정차수정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산품공정차수정보_생산품공정정보_생산품공정코드",
                        column: x => x.생산품공정코드,
                        principalTable: "생산품공정정보",
                        principalColumn: "생산품공정코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "공정단위검사장비",
                columns: table => new
                {
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    검사장비식별번호 = table.Column<int>(type: "int", nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_공정단위검사장비", x => new { x.공정단위코드, x.품질검사코드, x.검사장비식별번호 });
                    table.ForeignKey(
                        name: "FK_공정단위검사장비_공정단위검사정보_공정단위코드_품질검사코드",
                        columns: x => new { x.공정단위코드, x.품질검사코드 },
                        principalTable: "공정단위검사정보",
                        principalColumns: new[] { "공정단위코드", "품질검사코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정단위검사장비_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정단위검사장비_연동장비정보_검사장비식별번호",
                        column: x => x.검사장비식별번호,
                        principalTable: "연동장비정보",
                        principalColumn: "식별번호",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "생산실적상세정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    작업순번 = table.Column<int>(type: "int", nullable: false),
                    작업자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    사용품번 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    사용수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    실적등록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    일괄생산등록유무 = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_생산실적상세정보", x => new { x.회사코드, x.생산지시코드, x.작업순번 });
                    table.ForeignKey(
                        name: "FK_생산실적상세정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드");
                    table.ForeignKey(
                        name: "FK_생산실적상세정보_생산실적헤더정보_회사코드_생산지시코드",
                        columns: x => new { x.회사코드, x.생산지시코드 },
                        principalTable: "생산실적헤더정보",
                        principalColumns: new[] { "회사코드", "생산지시코드" });
                    table.ForeignKey(
                        name: "FK_생산실적상세정보_직원정보_회사코드_작업자사번",
                        columns: x => new { x.회사코드, x.작업자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" });
                });

            migrationBuilder.CreateTable(
                name: "보유품목일련정보",
                columns: table => new
                {
                    보유년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    생산년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    출고년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    일년번호 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    보유품목일지코드 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    보유명 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    보유일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_보유품목일련정보", x => new { x.보유년월일, x.순번, x.품목코드 });
                    table.ForeignKey(
                        name: "FK_보유품목일련정보_거래처정보_거래처회사코드_거래처코드1",
                        columns: x => new { x.거래처회사코드, x.거래처코드1 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목일련정보_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "보유품목일지",
                columns: table => new
                {
                    보유품목일지코드 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    보유년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    생산년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    출고년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    보유일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    거래처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_보유품목일지", x => x.보유품목일지코드);
                    table.ForeignKey(
                        name: "FK_보유품목일지_거래처정보_거래처회사코드_거래처코드1",
                        columns: x => new { x.거래처회사코드, x.거래처코드1 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목일지_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목일지_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획정보",
                columns: table => new
                {
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    생산품코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    발주처코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    납품일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    발주수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    계획수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    실행일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    종료일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    생산책임자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산계획상태코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    발주처회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    발주처거래처코드 = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    생산계획기본회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획기본생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획영업회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획영업생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획연구소회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획연구소생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획구매회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획구매생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획생산회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획생산생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획품질회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획품질생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획생산관리회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    생산계획생산관리생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
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
                    table.PrimaryKey("PK_생산계획정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획정보_거래처정보_발주처회사코드_발주처거래처코드",
                        columns: x => new { x.발주처회사코드, x.발주처거래처코드 },
                        principalTable: "거래처정보",
                        principalColumns: new[] { "회사코드", "거래처코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획정보_공통코드_생산계획상태코드",
                        column: x => x.생산계획상태코드,
                        principalTable: "공통코드",
                        principalColumn: "코드");
                    table.ForeignKey(
                        name: "FK_생산계획정보_공통코드_생산유형코드",
                        column: x => x.생산유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드");
                    table.ForeignKey(
                        name: "FK_생산계획정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드");
                    table.ForeignKey(
                        name: "FK_생산계획정보_생산품공정정보_생산품공정코드",
                        column: x => x.생산품공정코드,
                        principalTable: "생산품공정정보",
                        principalColumn: "생산품공정코드");
                    table.ForeignKey(
                        name: "FK_생산계획정보_직원정보_회사코드_생산책임자사번",
                        columns: x => new { x.회사코드, x.생산책임자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" });
                    table.ForeignKey(
                        name: "FK_생산계획정보_품목정보_생산품코드",
                        column: x => x.생산품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드");
                });

            migrationBuilder.CreateTable(
                name: "생산계획구매정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획구매정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획구매정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획구매정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획구매정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획구매정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획기본정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획기본정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획기본정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획기본정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획기본정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획기본정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획생산관리정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획생산관리정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획생산관리정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획생산관리정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획생산관리정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획생산관리정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획생산정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획생산정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획생산정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획생산정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획생산정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획생산정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획연구소정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획연구소정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획연구소정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획연구소정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획연구소정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획연구소정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획영업정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획영업정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획영업정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획영업정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획영업정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획영업정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산계획품질정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    사용유무 = table.Column<bool>(type: "bit", nullable: false),
                    삭제유무 = table.Column<bool>(type: "bit", nullable: false),
                    상세JSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    계획기록 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    계획기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    계획자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    검토자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    검토기록일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    전달사항 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_생산계획품질정보", x => new { x.회사코드, x.생산계획코드 });
                    table.ForeignKey(
                        name: "FK_생산계획품질정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획품질정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산계획품질정보_직원정보_회사코드_검토자사번",
                        columns: x => new { x.회사코드, x.검토자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산계획품질정보_직원정보_회사코드_계획자사번",
                        columns: x => new { x.회사코드, x.계획자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산지시정보",
                columns: table => new
                {
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    실행상태코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    생산계획코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    생산지시명 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    생산지시유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    생산수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    실생산량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    시작일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    완료목표일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    검사수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    합격수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    불량수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    생산계획정보생산계획코드 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    생산계획정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
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
                    table.PrimaryKey("PK_생산지시정보", x => x.생산지시코드);
                    table.ForeignKey(
                        name: "FK_생산지시정보_공통코드_생산지시유형코드",
                        column: x => x.생산지시유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산지시정보_공통코드_실행상태코드",
                        column: x => x.실행상태코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산지시정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산지시정보_생산계획정보_생산계획정보회사코드_생산계획정보생산계획코드",
                        columns: x => new { x.생산계획정보회사코드, x.생산계획정보생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산지시정보_생산계획정보_회사코드_생산계획코드",
                        columns: x => new { x.회사코드, x.생산계획코드 },
                        principalTable: "생산계획정보",
                        principalColumns: new[] { "회사코드", "생산계획코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "생산지시공정차수",
                columns: table => new
                {
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    공정차수 = table.Column<int>(type: "int", nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    생산품공정코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    생산품공정차수순번 = table.Column<int>(type: "int", nullable: false),
                    작업자사번 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    비고 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_생산지시공정차수", x => new { x.생산지시코드, x.공정차수 });
                    table.ForeignKey(
                        name: "FK_생산지시공정차수_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산지시공정차수_생산지시정보_생산지시코드",
                        column: x => x.생산지시코드,
                        principalTable: "생산지시정보",
                        principalColumn: "생산지시코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_생산지시공정차수_생산품공정차수정보_생산품공정코드_생산품공정차수순번",
                        columns: x => new { x.생산품공정코드, x.생산품공정차수순번 },
                        principalTable: "생산품공정차수정보",
                        principalColumns: new[] { "생산품공정코드", "순번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_생산지시공정차수_직원정보_회사코드_작업자사번",
                        columns: x => new { x.회사코드, x.작업자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목검사정보",
                columns: table => new
                {
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    측정값 = table.Column<decimal>(type: "decimal(7,3)", nullable: true),
                    측정유무 = table.Column<bool>(type: "bit", nullable: false),
                    검사결과코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    불량유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_보유품목검사정보", x => new { x.보유품목코드, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_보유품목검사정보_공정단위검사정보_공정단위코드_품질검사코드",
                        columns: x => new { x.공정단위코드, x.품질검사코드 },
                        principalTable: "공정단위검사정보",
                        principalColumns: new[] { "공정단위코드", "품질검사코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목검사정보_공통코드_검사결과코드",
                        column: x => x.검사결과코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목검사정보_공통코드_불량유형코드",
                        column: x => x.불량유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목검사정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목검사정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "공정단위설비정보",
                columns: table => new
                {
                    공정단위코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    설비코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
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
                    table.PrimaryKey("PK_공정단위설비정보", x => new { x.공정단위코드, x.설비코드 });
                    table.ForeignKey(
                        name: "FK_공정단위설비정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_공정단위설비정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "공정이력정보",
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
                    목표수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    생산수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    공정상태 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    시작일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    완료목표일 = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_공정이력정보", x => x.인덱스);
                    table.ForeignKey(
                        name: "FK_공정이력정보_공정단위정보_공정단위코드",
                        column: x => x.공정단위코드,
                        principalTable: "공정단위정보",
                        principalColumn: "공정단위코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정이력정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정이력정보_생산지시정보_생산지시코드",
                        column: x => x.생산지시코드,
                        principalTable: "생산지시정보",
                        principalColumn: "생산지시코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정이력정보_생산품공정정보_생산품공정코드",
                        column: x => x.생산품공정코드,
                        principalTable: "생산품공정정보",
                        principalColumn: "생산품공정코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정이력정보_직원정보_회사코드_작업자사번",
                        columns: x => new { x.회사코드, x.작업자사번 },
                        principalTable: "직원정보",
                        principalColumns: new[] { "회사코드", "사번" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_공정이력정보_품목정보_생산품코드",
                        column: x => x.생산품코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목불량정보",
                columns: table => new
                {
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    불량유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    불량등록일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    불량변경일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
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
                    table.PrimaryKey("PK_보유품목불량정보", x => new { x.보유품목코드, x.불량유형코드 });
                    table.ForeignKey(
                        name: "FK_보유품목불량정보_공통코드_불량유형코드",
                        column: x => x.불량유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_보유품목불량정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목이력",
                columns: table => new
                {
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    이력순번 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    변경유형코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    장소코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    장소위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    변경수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    변경사유 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    유형사유 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    연계보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    변경일시 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    위치상세코드 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    보유품목정보보유품목코드 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    보유품목정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
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
                    table.PrimaryKey("PK_보유품목이력", x => new { x.보유품목코드, x.이력순번 });
                    table.ForeignKey(
                        name: "FK_보유품목이력_공통코드_변경유형코드",
                        column: x => x.변경유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목이력_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목이력_장소위치정보_회사코드_장소위치코드",
                        columns: x => new { x.회사코드, x.장소위치코드 },
                        principalTable: "장소위치정보",
                        principalColumns: new[] { "회사코드", "장소위치코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목이력_장소정보_회사코드_장소코드",
                        columns: x => new { x.회사코드, x.장소코드 },
                        principalTable: "장소정보",
                        principalColumns: new[] { "회사코드", "장소코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목정보",
                columns: table => new
                {
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    보유년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    조정년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    품목구분코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    장소코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    장소위치코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(17,6)", nullable: false),
                    보유명 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    보유일 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOT번호 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    품목_LOT번호 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    설비가동현황회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    설비가동현황코드 = table.Column<string>(type: "nvarchar(30)", nullable: true),
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
                    table.PrimaryKey("PK_보유품목정보", x => new { x.회사코드, x.보유품목코드 });
                    table.ForeignKey(
                        name: "FK_보유품목정보_공통코드_품목구분코드",
                        column: x => x.품목구분코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_보유품목정보_장소위치정보_회사코드_장소위치코드",
                        columns: x => new { x.회사코드, x.장소위치코드 },
                        principalTable: "장소위치정보",
                        principalColumns: new[] { "회사코드", "장소위치코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목정보_장소정보_회사코드_장소코드",
                        columns: x => new { x.회사코드, x.장소코드 },
                        principalTable: "장소정보",
                        principalColumns: new[] { "회사코드", "장소코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목정보_품목정보_품목코드",
                        column: x => x.품목코드,
                        principalTable: "품목정보",
                        principalColumn: "품목코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목삭제일지",
                columns: table => new
                {
                    보유품목일지코드 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    순번 = table.Column<int>(type: "int", nullable: false),
                    보유품목코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    수량 = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    보유년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    생산년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    출고년월일 = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    보유일 = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_보유품목삭제일지", x => x.보유품목일지코드);
                    table.ForeignKey(
                        name: "FK_보유품목삭제일지_보유품목정보_회사코드_보유품목코드",
                        columns: x => new { x.회사코드, x.보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목삭제일지_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "보유품목위치정보",
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
                    보유품목정보보유품목코드 = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    보유품목정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    위치상세정보위치상세코드 = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    위치상세정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    장소위치정보장소위치코드 = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    장소위치정보회사코드 = table.Column<string>(type: "nvarchar(4)", nullable: true),
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
                    table.PrimaryKey("PK_보유품목위치정보", x => x.보유품목위치순번);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_보유품목정보_보유품목정보회사코드_보유품목정보보유품목코드",
                        columns: x => new { x.보유품목정보회사코드, x.보유품목정보보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_보유품목정보_회사코드_보유품목코드",
                        columns: x => new { x.회사코드, x.보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_위치상세정보_위치상세정보회사코드_위치상세정보위치상세코드",
                        columns: x => new { x.위치상세정보회사코드, x.위치상세정보위치상세코드 },
                        principalTable: "위치상세정보",
                        principalColumns: new[] { "회사코드", "위치상세코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_장소위치정보_장소위치정보회사코드_장소위치정보장소위치코드",
                        columns: x => new { x.장소위치정보회사코드, x.장소위치정보장소위치코드 },
                        principalTable: "장소위치정보",
                        principalColumns: new[] { "회사코드", "장소위치코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_보유품목위치정보_장소위치정보_회사코드_장소위치코드",
                        columns: x => new { x.회사코드, x.장소위치코드 },
                        principalTable: "장소위치정보",
                        principalColumns: new[] { "회사코드", "장소위치코드" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "설비가동현황정보",
                columns: table => new
                {
                    코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    상태 = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    상태유형코드 = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    이전상태 = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    설비코드 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    상태변경시각 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    상태유지시간Ticks = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_설비가동현황정보", x => new { x.회사코드, x.코드 });
                    table.ForeignKey(
                        name: "FK_설비가동현황정보_공통코드_상태유형코드",
                        column: x => x.상태유형코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_설비가동현황정보_보유품목정보_회사코드_설비코드",
                        columns: x => new { x.회사코드, x.설비코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_설비가동현황정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "품질검사측정정보",
                columns: table => new
                {
                    시리얼넘버 = table.Column<int>(type: "int", nullable: false),
                    품질검사코드 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    회사코드 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    생산지시코드 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_품질검사측정정보", x => new { x.시리얼넘버, x.품질검사코드 });
                    table.ForeignKey(
                        name: "FK_품질검사측정정보_공통코드_검사단위코드",
                        column: x => x.검사단위코드,
                        principalTable: "공통코드",
                        principalColumn: "코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                        columns: x => new { x.회사코드, x.보유품목코드 },
                        principalTable: "보유품목정보",
                        principalColumns: new[] { "회사코드", "보유품목코드" },
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_품질검사측정정보_사업장_회사코드",
                        column: x => x.회사코드,
                        principalTable: "사업장",
                        principalColumn: "회사코드",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_품질검사측정정보_품질검사정보_품질검사코드",
                        column: x => x.품질검사코드,
                        principalTable: "품질검사정보",
                        principalColumn: "품질검사코드",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_거래처정보_거래처구분코드",
                table: "거래처정보",
                column: "거래처구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위검사장비_검사장비식별번호",
                table: "공정단위검사장비",
                column: "검사장비식별번호");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위검사장비_회사코드",
                table: "공정단위검사장비",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위검사정보_검사단위코드",
                table: "공정단위검사정보",
                column: "검사단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위검사정보_품질검사코드",
                table: "공정단위검사정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위검사정보_회사코드",
                table: "공정단위검사정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위설비정보_회사코드_설비코드",
                table: "공정단위설비정보",
                columns: new[] { "회사코드", "설비코드" });

            migrationBuilder.CreateIndex(
                name: "IX_공정단위자재정보_자재코드",
                table: "공정단위자재정보",
                column: "자재코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위자재정보_회사코드",
                table: "공정단위자재정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_공정코드",
                table: "공정단위정보",
                column: "공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_공정품유형코드",
                table: "공정단위정보",
                column: "공정품유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_공정품코드",
                table: "공정단위정보",
                column: "공정품코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_도면코드",
                table: "공정단위정보",
                column: "도면코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_완제품코드",
                table: "공정단위정보",
                column: "완제품코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_원공정단위코드_관리차수",
                table: "공정단위정보",
                columns: new[] { "원공정단위코드", "관리차수" },
                unique: true,
                filter: "[원공정단위코드] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_공정단위정보_회사코드",
                table: "공정단위정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_공정단위코드",
                table: "공정이력정보",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_생산지시코드",
                table: "공정이력정보",
                column: "생산지시코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_생산품공정코드",
                table: "공정이력정보",
                column: "생산품공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_생산품코드",
                table: "공정이력정보",
                column: "생산품코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_회사코드_설비코드",
                table: "공정이력정보",
                columns: new[] { "회사코드", "설비코드" });

            migrationBuilder.CreateIndex(
                name: "IX_공정이력정보_회사코드_작업자사번",
                table: "공정이력정보",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_공정정보_공정유형코드",
                table: "공정정보",
                column: "공정유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_공정정보_설비유형코드",
                table: "공정정보",
                column: "설비유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_공통코드_상위코드",
                table: "공통코드",
                column: "상위코드");

            migrationBuilder.CreateIndex(
                name: "IX_공통코드_코드명",
                table: "공통코드",
                column: "코드명",
                unique: true,
                filter: "[코드명] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_공통코드_코드유형코드",
                table: "공통코드",
                column: "코드유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_도면정보_도면종류코드",
                table: "도면정보",
                column: "도면종류코드");

            migrationBuilder.CreateIndex(
                name: "IX_도면정보_파일폴더순번",
                table: "도면정보",
                column: "파일폴더순번");

            migrationBuilder.CreateIndex(
                name: "IX_메뉴부서권한정보_회사코드_부서코드",
                table: "메뉴부서권한정보",
                columns: new[] { "회사코드", "부서코드" });

            migrationBuilder.CreateIndex(
                name: "IX_메뉴유형별권한정보_권한유형코드",
                table: "메뉴유형별권한정보",
                column: "권한유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_메뉴정보_상위메뉴순번",
                table: "메뉴정보",
                column: "상위메뉴순번");

            migrationBuilder.CreateIndex(
                name: "IX_메뉴직원권한정보_회사코드_직원사번",
                table: "메뉴직원권한정보",
                columns: new[] { "회사코드", "직원사번" });

            migrationBuilder.CreateIndex(
                name: "IX_메시지정보_메시지유형코드",
                table: "메시지정보",
                column: "메시지유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_메시지정보_회사코드_발송인사번",
                table: "메시지정보",
                columns: new[] { "회사코드", "발송인사번" });

            migrationBuilder.CreateIndex(
                name: "IX_메시지정보_회사코드_수신인사번",
                table: "메시지정보",
                columns: new[] { "회사코드", "수신인사번" });

            migrationBuilder.CreateIndex(
                name: "IX_바코드발급정보_품목코드",
                table: "바코드발급정보",
                column: "품목코드");

            migrationBuilder.CreateIndex(
                name: "IX_바코드발급정보_회사코드",
                table: "바코드발급정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주정보_거래처코드",
                table: "발주정보",
                column: "거래처코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주정보_거래처회사코드_거래처코드1",
                table: "발주정보",
                columns: new[] { "거래처회사코드", "거래처코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_발주정보상세_발주순번",
                table: "발주정보상세",
                column: "발주순번");

            migrationBuilder.CreateIndex(
                name: "IX_발주정보상세_발주정보발주순번_발주정보회사코드",
                table: "발주정보상세",
                columns: new[] { "발주정보발주순번", "발주정보회사코드" });

            migrationBuilder.CreateIndex(
                name: "IX_발주정보상세_품목구분코드",
                table: "발주정보상세",
                column: "품목구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_발주정보상세_품목코드",
                table: "발주정보상세",
                column: "품목코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목검사정보_검사결과코드",
                table: "보유품목검사정보",
                column: "검사결과코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목검사정보_공정단위코드_품질검사코드",
                table: "보유품목검사정보",
                columns: new[] { "공정단위코드", "품질검사코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목검사정보_불량유형코드",
                table: "보유품목검사정보",
                column: "불량유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목검사정보_품질검사코드",
                table: "보유품목검사정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목검사정보_회사코드_보유품목코드",
                table: "보유품목검사정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목불량정보_불량유형코드",
                table: "보유품목불량정보",
                column: "불량유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목불량정보_회사코드_보유품목코드",
                table: "보유품목불량정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목삭제일지_회사코드_보유품목코드",
                table: "보유품목삭제일지",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목위치정보_보유품목정보회사코드_보유품목정보보유품목코드",
                table: "보유품목위치정보",
                columns: new[] { "보유품목정보회사코드", "보유품목정보보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목위치정보_위치상세정보회사코드_위치상세정보위치상세코드",
                table: "보유품목위치정보",
                columns: new[] { "위치상세정보회사코드", "위치상세정보위치상세코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목위치정보_장소위치정보회사코드_장소위치정보장소위치코드",
                table: "보유품목위치정보",
                columns: new[] { "장소위치정보회사코드", "장소위치정보장소위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목위치정보_회사코드_보유품목코드",
                table: "보유품목위치정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목위치정보_회사코드_장소위치코드",
                table: "보유품목위치정보",
                columns: new[] { "회사코드", "장소위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목이력_변경유형코드",
                table: "보유품목이력",
                column: "변경유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목이력_보유품목정보회사코드_보유품목정보보유품목코드",
                table: "보유품목이력",
                columns: new[] { "보유품목정보회사코드", "보유품목정보보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목이력_회사코드_연계보유품목코드",
                table: "보유품목이력",
                columns: new[] { "회사코드", "연계보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목이력_회사코드_장소위치코드",
                table: "보유품목이력",
                columns: new[] { "회사코드", "장소위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목이력_회사코드_장소코드",
                table: "보유품목이력",
                columns: new[] { "회사코드", "장소코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일련정보_거래처코드",
                table: "보유품목일련정보",
                column: "거래처코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일련정보_거래처회사코드_거래처코드1",
                table: "보유품목일련정보",
                columns: new[] { "거래처회사코드", "거래처코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일련정보_보유품목일지코드",
                table: "보유품목일련정보",
                column: "보유품목일지코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일련정보_생산지시코드",
                table: "보유품목일련정보",
                column: "생산지시코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일련정보_품목코드",
                table: "보유품목일련정보",
                column: "품목코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일지_거래처회사코드_거래처코드1",
                table: "보유품목일지",
                columns: new[] { "거래처회사코드", "거래처코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일지_보유품목일지코드",
                table: "보유품목일지",
                column: "보유품목일지코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일지_품목코드",
                table: "보유품목일지",
                column: "품목코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목일지_회사코드_보유품목코드",
                table: "보유품목일지",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목임시위치정보_위치상세코드",
                table: "보유품목임시위치정보",
                column: "위치상세코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목정보_설비가동현황회사코드_설비가동현황코드",
                table: "보유품목정보",
                columns: new[] { "설비가동현황회사코드", "설비가동현황코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목정보_품목구분코드",
                table: "보유품목정보",
                column: "품목구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_보유품목정보_품목코드_보유년월일_순번",
                table: "보유품목정보",
                columns: new[] { "품목코드", "보유년월일", "순번" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목정보_회사코드_장소위치코드",
                table: "보유품목정보",
                columns: new[] { "회사코드", "장소위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_보유품목정보_회사코드_장소코드",
                table: "보유품목정보",
                columns: new[] { "회사코드", "장소코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획구매정보_회사코드_검토자사번",
                table: "생산계획구매정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획구매정보_회사코드_계획자사번",
                table: "생산계획구매정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획기본정보_회사코드_검토자사번",
                table: "생산계획기본정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획기본정보_회사코드_계획자사번",
                table: "생산계획기본정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획생산관리정보_회사코드_검토자사번",
                table: "생산계획생산관리정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획생산관리정보_회사코드_계획자사번",
                table: "생산계획생산관리정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획생산정보_회사코드_검토자사번",
                table: "생산계획생산정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획생산정보_회사코드_계획자사번",
                table: "생산계획생산정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획연구소정보_회사코드_검토자사번",
                table: "생산계획연구소정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획연구소정보_회사코드_계획자사번",
                table: "생산계획연구소정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획영업정보_회사코드_검토자사번",
                table: "생산계획영업정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획영업정보_회사코드_계획자사번",
                table: "생산계획영업정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_발주처코드",
                table: "생산계획정보",
                column: "발주처코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_발주처회사코드_발주처거래처코드",
                table: "생산계획정보",
                columns: new[] { "발주처회사코드", "발주처거래처코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획구매회사코드_생산계획구매생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획구매회사코드", "생산계획구매생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획기본회사코드_생산계획기본생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획기본회사코드", "생산계획기본생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획상태코드",
                table: "생산계획정보",
                column: "생산계획상태코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획생산관리회사코드_생산계획생산관리생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획생산관리회사코드", "생산계획생산관리생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획생산회사코드_생산계획생산생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획생산회사코드", "생산계획생산생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획연구소회사코드_생산계획연구소생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획연구소회사코드", "생산계획연구소생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획영업회사코드_생산계획영업생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획영업회사코드", "생산계획영업생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산계획품질회사코드_생산계획품질생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획품질회사코드", "생산계획품질생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산유형코드",
                table: "생산계획정보",
                column: "생산유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산품공정코드",
                table: "생산계획정보",
                column: "생산품공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_생산품코드",
                table: "생산계획정보",
                column: "생산품코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산계획정보_회사코드_생산책임자사번",
                table: "생산계획정보",
                columns: new[] { "회사코드", "생산책임자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획품질정보_회사코드_검토자사번",
                table: "생산계획품질정보",
                columns: new[] { "회사코드", "검토자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산계획품질정보_회사코드_계획자사번",
                table: "생산계획품질정보",
                columns: new[] { "회사코드", "계획자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산실적상세정보_회사코드_작업자사번",
                table: "생산실적상세정보",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산실적헤더정보_공정단위코드",
                table: "생산실적헤더정보",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산실적헤더정보_생산품공정코드",
                table: "생산실적헤더정보",
                column: "생산품공정코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산실적헤더정보_생산품코드",
                table: "생산실적헤더정보",
                column: "생산품코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산지시공정차수_생산품공정코드_생산품공정차수순번",
                table: "생산지시공정차수",
                columns: new[] { "생산품공정코드", "생산품공정차수순번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산지시공정차수_회사코드_작업자사번",
                table: "생산지시공정차수",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_생산지시정보_생산계획정보회사코드_생산계획정보생산계획코드",
                table: "생산지시정보",
                columns: new[] { "생산계획정보회사코드", "생산계획정보생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산지시정보_생산계획코드_순번",
                table: "생산지시정보",
                columns: new[] { "생산계획코드", "순번" },
                unique: true,
                filter: "[생산계획코드] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_생산지시정보_생산지시유형코드",
                table: "생산지시정보",
                column: "생산지시유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산지시정보_실행상태코드",
                table: "생산지시정보",
                column: "실행상태코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산지시정보_회사코드_생산계획코드",
                table: "생산지시정보",
                columns: new[] { "회사코드", "생산계획코드" });

            migrationBuilder.CreateIndex(
                name: "IX_생산품공정정보_생산품코드_관리차수",
                table: "생산품공정정보",
                columns: new[] { "생산품코드", "관리차수" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_생산품공정정보_회사코드",
                table: "생산품공정정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산품공정차수정보_공정단위정보공정단위코드",
                table: "생산품공정차수정보",
                column: "공정단위정보공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산품공정차수정보_공정단위코드",
                table: "생산품공정차수정보",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_생산품공정차수정보_회사코드",
                table: "생산품공정차수정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_설비가동현황정보_상태유형코드",
                table: "설비가동현황정보",
                column: "상태유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_설비가동현황정보_회사코드_설비코드",
                table: "설비가동현황정보",
                columns: new[] { "회사코드", "설비코드" });

            migrationBuilder.CreateIndex(
                name: "IX_액션로그_액션코드",
                table: "액션로그",
                column: "액션코드");

            migrationBuilder.CreateIndex(
                name: "IX_액션로그_연동장비식별번호",
                table: "액션로그",
                column: "연동장비식별번호");

            migrationBuilder.CreateIndex(
                name: "IX_액션로그_회사코드_순번",
                table: "액션로그",
                columns: new[] { "회사코드", "순번" });

            migrationBuilder.CreateIndex(
                name: "IX_액션로그_회사코드_직원사번",
                table: "액션로그",
                columns: new[] { "회사코드", "직원사번" });

            migrationBuilder.CreateIndex(
                name: "IX_액션정보_대체액션코드",
                table: "액션정보",
                column: "대체액션코드");

            migrationBuilder.CreateIndex(
                name: "IX_액션정보_액션유형코드",
                table: "액션정보",
                column: "액션유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_연동장비정보_연동장비유형코드",
                table: "연동장비정보",
                column: "연동장비유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_위치상세정보_위치상세분류코드",
                table: "위치상세정보",
                column: "위치상세분류코드");

            migrationBuilder.CreateIndex(
                name: "IX_위치상세정보_회사코드_장소위치코드",
                table: "위치상세정보",
                columns: new[] { "회사코드", "장소위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_작업자생산실적정보_회사코드_작업자사번",
                table: "작업자생산실적정보",
                columns: new[] { "회사코드", "작업자사번" });

            migrationBuilder.CreateIndex(
                name: "IX_장소위치정보_위치분류코드",
                table: "장소위치정보",
                column: "위치분류코드");

            migrationBuilder.CreateIndex(
                name: "IX_장소위치정보_장소코드_위치코드",
                table: "장소위치정보",
                columns: new[] { "장소코드", "위치코드" });

            migrationBuilder.CreateIndex(
                name: "IX_장소위치정보_회사코드_장소코드",
                table: "장소위치정보",
                columns: new[] { "회사코드", "장소코드" });

            migrationBuilder.CreateIndex(
                name: "IX_장소정보_공정구분코드",
                table: "장소정보",
                column: "공정구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_장소정보_장소유형코드",
                table: "장소정보",
                column: "장소유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_직원권한정보_회사코드",
                table: "직원권한정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_권한코드",
                table: "직원정보",
                column: "권한코드");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_식별번호",
                table: "직원정보",
                column: "식별번호",
                unique: true,
                filter: "[식별번호] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_식별인자",
                table: "직원정보",
                column: "식별인자");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_직급코드",
                table: "직원정보",
                column: "직급코드");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_직책코드",
                table: "직원정보",
                column: "직책코드");

            migrationBuilder.CreateIndex(
                name: "IX_직원정보_회사코드_부서코드",
                table: "직원정보",
                columns: new[] { "회사코드", "부서코드" });

            migrationBuilder.CreateIndex(
                name: "IX_파일정보_폴더순번",
                table: "파일정보",
                column: "폴더순번");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_거래처코드",
                table: "품목정보",
                column: "거래처코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_거래처회사코드_거래처코드1",
                table: "품목정보",
                columns: new[] { "거래처회사코드", "거래처코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_규격종류코드",
                table: "품목정보",
                column: "규격종류코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_단위코드",
                table: "품목정보",
                column: "단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_소재코드",
                table: "품목정보",
                column: "소재코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_원품목코드_관리차수",
                table: "품목정보",
                columns: new[] { "원품목코드", "관리차수" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_조달구분코드",
                table: "품목정보",
                column: "조달구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_품목구분코드",
                table: "품목정보",
                column: "품목구분코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_품목유형코드",
                table: "품목정보",
                column: "품목유형코드");

            migrationBuilder.CreateIndex(
                name: "IX_품목정보_회사코드",
                table: "품목정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_검사단위코드",
                table: "품질검사측정정보",
                column: "검사단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_품질검사코드",
                table: "품질검사측정정보",
                column: "품질검사코드");

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_회사코드_보유품목코드",
                table: "품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_BOM정보_상위BOM순번",
                table: "BOM정보",
                column: "상위BOM순번");

            migrationBuilder.CreateIndex(
                name: "IX_BOM정보_품목코드",
                table: "BOM정보",
                column: "품목코드");

            migrationBuilder.CreateIndex(
                name: "IX_BOM품목정보상세_공정단위정보공정단위코드",
                table: "BOM품목정보상세",
                column: "공정단위정보공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_BOM품목정보상세_공정단위코드",
                table: "BOM품목정보상세",
                column: "공정단위코드");

            migrationBuilder.CreateIndex(
                name: "IX_BOM품목정보상세_BOM품목정보코드",
                table: "BOM품목정보상세",
                column: "BOM품목정보코드");

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목일련정보_보유품목일지_보유품목일지코드",
                table: "보유품목일련정보",
                column: "보유품목일지코드",
                principalTable: "보유품목일지",
                principalColumn: "보유품목일지코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목일련정보_생산지시정보_생산지시코드",
                table: "보유품목일련정보",
                column: "생산지시코드",
                principalTable: "생산지시정보",
                principalColumn: "생산지시코드",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목일지_보유품목정보_회사코드_보유품목코드",
                table: "보유품목일지",
                columns: new[] { "회사코드", "보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획구매정보_생산계획구매회사코드_생산계획구매생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획구매회사코드", "생산계획구매생산계획코드" },
                principalTable: "생산계획구매정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획기본정보_생산계획기본회사코드_생산계획기본생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획기본회사코드", "생산계획기본생산계획코드" },
                principalTable: "생산계획기본정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획생산관리정보_생산계획생산관리회사코드_생산계획생산관리생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획생산관리회사코드", "생산계획생산관리생산계획코드" },
                principalTable: "생산계획생산관리정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획생산정보_생산계획생산회사코드_생산계획생산생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획생산회사코드", "생산계획생산생산계획코드" },
                principalTable: "생산계획생산정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획연구소정보_생산계획연구소회사코드_생산계획연구소생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획연구소회사코드", "생산계획연구소생산계획코드" },
                principalTable: "생산계획연구소정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획영업정보_생산계획영업회사코드_생산계획영업생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획영업회사코드", "생산계획영업생산계획코드" },
                principalTable: "생산계획영업정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_생산계획정보_생산계획품질정보_생산계획품질회사코드_생산계획품질생산계획코드",
                table: "생산계획정보",
                columns: new[] { "생산계획품질회사코드", "생산계획품질생산계획코드" },
                principalTable: "생산계획품질정보",
                principalColumns: new[] { "회사코드", "생산계획코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목검사정보_보유품목정보_회사코드_보유품목코드",
                table: "보유품목검사정보",
                columns: new[] { "회사코드", "보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정단위설비정보_보유품목정보_회사코드_설비코드",
                table: "공정단위설비정보",
                columns: new[] { "회사코드", "설비코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_공정이력정보_보유품목정보_회사코드_설비코드",
                table: "공정이력정보",
                columns: new[] { "회사코드", "설비코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목불량정보_보유품목정보_회사코드_보유품목코드",
                table: "보유품목불량정보",
                columns: new[] { "회사코드", "보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목이력_보유품목정보_보유품목정보회사코드_보유품목정보보유품목코드",
                table: "보유품목이력",
                columns: new[] { "보유품목정보회사코드", "보유품목정보보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목이력_보유품목정보_회사코드_연계보유품목코드",
                table: "보유품목이력",
                columns: new[] { "회사코드", "연계보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_보유품목정보_설비가동현황정보_설비가동현황회사코드_설비가동현황코드",
                table: "보유품목정보",
                columns: new[] { "설비가동현황회사코드", "설비가동현황코드" },
                principalTable: "설비가동현황정보",
                principalColumns: new[] { "회사코드", "코드" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_거래처정보_공통코드_거래처구분코드",
                table: "거래처정보");

            migrationBuilder.DropForeignKey(
                name: "FK_보유품목정보_공통코드_품목구분코드",
                table: "보유품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_공통코드_생산계획상태코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_공통코드_생산유형코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_설비가동현황정보_공통코드_상태유형코드",
                table: "설비가동현황정보");

            migrationBuilder.DropForeignKey(
                name: "FK_장소위치정보_공통코드_위치분류코드",
                table: "장소위치정보");

            migrationBuilder.DropForeignKey(
                name: "FK_장소정보_공통코드_공정구분코드",
                table: "장소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_장소정보_공통코드_장소유형코드",
                table: "장소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_직원정보_공통코드_권한코드",
                table: "직원정보");

            migrationBuilder.DropForeignKey(
                name: "FK_직원정보_공통코드_직급코드",
                table: "직원정보");

            migrationBuilder.DropForeignKey(
                name: "FK_직원정보_공통코드_직책코드",
                table: "직원정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_규격종류코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_단위코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_소재코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_조달구분코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_품목구분코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_공통코드_품목유형코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_보유품목정보_사업장_회사코드",
                table: "보유품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_부서정보_사업장_회사코드",
                table: "부서정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획구매정보_사업장_회사코드",
                table: "생산계획구매정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획기본정보_사업장_회사코드",
                table: "생산계획기본정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산관리정보_사업장_회사코드",
                table: "생산계획생산관리정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산정보_사업장_회사코드",
                table: "생산계획생산정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획연구소정보_사업장_회사코드",
                table: "생산계획연구소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획영업정보_사업장_회사코드",
                table: "생산계획영업정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_사업장_회사코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획품질정보_사업장_회사코드",
                table: "생산계획품질정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산품공정정보_사업장_회사코드",
                table: "생산품공정정보");

            migrationBuilder.DropForeignKey(
                name: "FK_설비가동현황정보_사업장_회사코드",
                table: "설비가동현황정보");

            migrationBuilder.DropForeignKey(
                name: "FK_장소위치정보_사업장_회사코드",
                table: "장소위치정보");

            migrationBuilder.DropForeignKey(
                name: "FK_장소정보_사업장_회사코드",
                table: "장소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_직원권한정보_사업장_회사코드",
                table: "직원권한정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품목정보_사업장_회사코드",
                table: "품목정보");

            migrationBuilder.DropForeignKey(
                name: "FK_설비가동현황정보_보유품목정보_회사코드_설비코드",
                table: "설비가동현황정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_품목정보_생산품코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산품공정정보_품목정보_생산품코드",
                table: "생산품공정정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_생산품공정정보_생산품공정코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획구매정보_직원정보_회사코드_검토자사번",
                table: "생산계획구매정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획구매정보_직원정보_회사코드_계획자사번",
                table: "생산계획구매정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획기본정보_직원정보_회사코드_검토자사번",
                table: "생산계획기본정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획기본정보_직원정보_회사코드_계획자사번",
                table: "생산계획기본정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산관리정보_직원정보_회사코드_검토자사번",
                table: "생산계획생산관리정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산관리정보_직원정보_회사코드_계획자사번",
                table: "생산계획생산관리정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산정보_직원정보_회사코드_검토자사번",
                table: "생산계획생산정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산정보_직원정보_회사코드_계획자사번",
                table: "생산계획생산정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획연구소정보_직원정보_회사코드_검토자사번",
                table: "생산계획연구소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획연구소정보_직원정보_회사코드_계획자사번",
                table: "생산계획연구소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획영업정보_직원정보_회사코드_검토자사번",
                table: "생산계획영업정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획영업정보_직원정보_회사코드_계획자사번",
                table: "생산계획영업정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_직원정보_회사코드_생산책임자사번",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획품질정보_직원정보_회사코드_검토자사번",
                table: "생산계획품질정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획품질정보_직원정보_회사코드_계획자사번",
                table: "생산계획품질정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획정보_거래처정보_발주처회사코드_발주처거래처코드",
                table: "생산계획정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획구매정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획구매정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획기본정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획기본정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산관리정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획생산관리정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획생산정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획생산정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획연구소정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획연구소정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획영업정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획영업정보");

            migrationBuilder.DropForeignKey(
                name: "FK_생산계획품질정보_생산계획정보_회사코드_생산계획코드",
                table: "생산계획품질정보");

            migrationBuilder.DropTable(
                name: "공정단위검사장비");

            migrationBuilder.DropTable(
                name: "공정단위설비정보");

            migrationBuilder.DropTable(
                name: "공정단위자재정보");

            migrationBuilder.DropTable(
                name: "공정이력정보");

            migrationBuilder.DropTable(
                name: "메뉴부서권한정보");

            migrationBuilder.DropTable(
                name: "메뉴유형별권한정보");

            migrationBuilder.DropTable(
                name: "메뉴직원권한정보");

            migrationBuilder.DropTable(
                name: "메시지정보");

            migrationBuilder.DropTable(
                name: "물류담당자정보");

            migrationBuilder.DropTable(
                name: "바코드발급정보");

            migrationBuilder.DropTable(
                name: "발주서정보");

            migrationBuilder.DropTable(
                name: "발주정보상세");

            migrationBuilder.DropTable(
                name: "보유품목검사정보");

            migrationBuilder.DropTable(
                name: "보유품목불량정보");

            migrationBuilder.DropTable(
                name: "보유품목삭제일지");

            migrationBuilder.DropTable(
                name: "보유품목위치정보");

            migrationBuilder.DropTable(
                name: "보유품목이력");

            migrationBuilder.DropTable(
                name: "보유품목일련정보");

            migrationBuilder.DropTable(
                name: "보유품목임시위치정보");

            migrationBuilder.DropTable(
                name: "사용자재보고정보");

            migrationBuilder.DropTable(
                name: "생산실적상세정보");

            migrationBuilder.DropTable(
                name: "생산지시공정차수");

            migrationBuilder.DropTable(
                name: "액션로그");

            migrationBuilder.DropTable(
                name: "외주작업지시서정보");

            migrationBuilder.DropTable(
                name: "일괄생산실적상세정보");

            migrationBuilder.DropTable(
                name: "입고처리상세정보");

            migrationBuilder.DropTable(
                name: "작업외주생산실적등록정보");

            migrationBuilder.DropTable(
                name: "작업자생산실적정보");

            migrationBuilder.DropTable(
                name: "재고이동상세정보");

            migrationBuilder.DropTable(
                name: "재고조정정보");

            migrationBuilder.DropTable(
                name: "재고조정정보이력");

            migrationBuilder.DropTable(
                name: "주문서정보");

            migrationBuilder.DropTable(
                name: "출고처리상세정보");

            migrationBuilder.DropTable(
                name: "파일정보");

            migrationBuilder.DropTable(
                name: "품질검사측정정보");

            migrationBuilder.DropTable(
                name: "BOM_정보");

            migrationBuilder.DropTable(
                name: "BOM정보");

            migrationBuilder.DropTable(
                name: "BOM품목정보상세");

            migrationBuilder.DropTable(
                name: "메뉴정보");

            migrationBuilder.DropTable(
                name: "발주서헤더정보");

            migrationBuilder.DropTable(
                name: "발주정보");

            migrationBuilder.DropTable(
                name: "공정단위검사정보");

            migrationBuilder.DropTable(
                name: "위치상세정보");

            migrationBuilder.DropTable(
                name: "보유품목일지");

            migrationBuilder.DropTable(
                name: "생산실적헤더정보");

            migrationBuilder.DropTable(
                name: "생산지시정보");

            migrationBuilder.DropTable(
                name: "생산품공정차수정보");

            migrationBuilder.DropTable(
                name: "액션정보");

            migrationBuilder.DropTable(
                name: "연동장비정보");

            migrationBuilder.DropTable(
                name: "외주작업지시헤더정보");

            migrationBuilder.DropTable(
                name: "일괄생산실적헤더정보");

            migrationBuilder.DropTable(
                name: "입고처리헤더정보");

            migrationBuilder.DropTable(
                name: "재고이동헤더정보");

            migrationBuilder.DropTable(
                name: "주문서헤더정보");

            migrationBuilder.DropTable(
                name: "출고처리헤더정보");

            migrationBuilder.DropTable(
                name: "BOM품목정보");

            migrationBuilder.DropTable(
                name: "품질검사정보");

            migrationBuilder.DropTable(
                name: "공정단위정보");

            migrationBuilder.DropTable(
                name: "공정정보");

            migrationBuilder.DropTable(
                name: "도면정보");

            migrationBuilder.DropTable(
                name: "파일폴더정보");

            migrationBuilder.DropTable(
                name: "공통코드");

            migrationBuilder.DropTable(
                name: "사업장");

            migrationBuilder.DropTable(
                name: "보유품목정보");

            migrationBuilder.DropTable(
                name: "설비가동현황정보");

            migrationBuilder.DropTable(
                name: "장소위치정보");

            migrationBuilder.DropTable(
                name: "장소정보");

            migrationBuilder.DropTable(
                name: "품목정보");

            migrationBuilder.DropTable(
                name: "생산품공정정보");

            migrationBuilder.DropTable(
                name: "직원정보");

            migrationBuilder.DropTable(
                name: "부서정보");

            migrationBuilder.DropTable(
                name: "직원권한정보");

            migrationBuilder.DropTable(
                name: "거래처정보");

            migrationBuilder.DropTable(
                name: "생산계획정보");

            migrationBuilder.DropTable(
                name: "생산계획구매정보");

            migrationBuilder.DropTable(
                name: "생산계획기본정보");

            migrationBuilder.DropTable(
                name: "생산계획생산관리정보");

            migrationBuilder.DropTable(
                name: "생산계획생산정보");

            migrationBuilder.DropTable(
                name: "생산계획연구소정보");

            migrationBuilder.DropTable(
                name: "생산계획영업정보");

            migrationBuilder.DropTable(
                name: "생산계획품질정보");
        }
    }
}
