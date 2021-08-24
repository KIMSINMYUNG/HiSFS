using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021071201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOMALL_정보",
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
                    table.PrimaryKey("PK_BOMALL_정보", x => new { x.회사코드, x.모품번, x.순번 });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOMALL_정보");
        }
    }
}
