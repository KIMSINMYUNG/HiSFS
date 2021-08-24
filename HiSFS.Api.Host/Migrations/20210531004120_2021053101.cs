using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateId",
                table: "외주생산위치정보",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "외주생산위치정보",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateId",
                table: "외주생산위치정보",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "외주생산위치정보",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "사용유무",
                table: "외주생산위치정보",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "삭제유무",
                table: "외주생산위치정보",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "상세JSON",
                table: "외주생산위치정보",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateId",
                table: "보유품목임시위치정보",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "보유품목임시위치정보",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateId",
                table: "보유품목임시위치정보",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "보유품목임시위치정보",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "사용유무",
                table: "보유품목임시위치정보",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "삭제유무",
                table: "보유품목임시위치정보",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "상세JSON",
                table: "보유품목임시위치정보",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateId",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "UpdateId",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "사용유무",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "삭제유무",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "상세JSON",
                table: "외주생산위치정보");

            migrationBuilder.DropColumn(
                name: "CreateId",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "UpdateId",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "사용유무",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "삭제유무",
                table: "보유품목임시위치정보");

            migrationBuilder.DropColumn(
                name: "상세JSON",
                table: "보유품목임시위치정보");
        }
    }
}
