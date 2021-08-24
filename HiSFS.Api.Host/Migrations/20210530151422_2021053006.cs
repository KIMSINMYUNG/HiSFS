using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "검사수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<decimal>(
                name: "합격수량",
                table: "외주작업지시서정보",
                type: "decimal(7,3)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "검사수량",
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
                name: "합격수량",
                table: "외주작업지시서정보");
        }
    }
}
