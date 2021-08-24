using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021060102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_외주품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                table: "외주품질검사측정정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                table: "품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_품질검사측정정보",
                table: "품질검사측정정보");

            //migrationBuilder.DropIndex(
            //    name: "IX_품질검사측정정보_회사코드_보유품목코드",
            //    table: "품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_외주품질검사측정정보",
                table: "외주품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_외주품질검사측정정보_회사코드_보유품목코드",
                table: "외주품질검사측정정보");

            migrationBuilder.AlterColumn<string>(
                name: "생산지시코드",
                table: "품질검사측정정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "보유품목코드1",
                table: "품질검사측정정보",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "보유품목회사코드",
                table: "품질검사측정정보",
                type: "nvarchar(4)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "지시번호",
                table: "외주품질검사측정정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "보유품목코드1",
                table: "외주품질검사측정정보",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "보유품목회사코드",
                table: "외주품질검사측정정보",
                type: "nvarchar(4)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_품질검사측정정보",
                table: "품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드", "생산지시코드" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_외주품질검사측정정보",
                table: "외주품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드", "지시번호" });

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_회사코드",
                table: "품질검사측정정보",
                column: "회사코드");

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "외주품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" });

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_회사코드",
                table: "외주품질검사측정정보",
                column: "회사코드");

            migrationBuilder.AddForeignKey(
                name: "FK_외주품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "외주품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보",
                columns: new[] { "보유품목회사코드", "보유품목코드1" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_외주품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "외주품질검사측정정보");

            migrationBuilder.DropForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_품질검사측정정보",
                table: "품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_품질검사측정정보_회사코드",
                table: "품질검사측정정보");

            migrationBuilder.DropPrimaryKey(
                name: "PK_외주품질검사측정정보",
                table: "외주품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_외주품질검사측정정보_보유품목회사코드_보유품목코드1",
                table: "외주품질검사측정정보");

            migrationBuilder.DropIndex(
                name: "IX_외주품질검사측정정보_회사코드",
                table: "외주품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목코드1",
                table: "품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목회사코드",
                table: "품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목코드1",
                table: "외주품질검사측정정보");

            migrationBuilder.DropColumn(
                name: "보유품목회사코드",
                table: "외주품질검사측정정보");

            migrationBuilder.AlterColumn<string>(
                name: "생산지시코드",
                table: "품질검사측정정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "지시번호",
                table: "외주품질검사측정정보",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_품질검사측정정보",
                table: "품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_외주품질검사측정정보",
                table: "외주품질검사측정정보",
                columns: new[] { "시리얼넘버", "품질검사코드" });

            migrationBuilder.CreateIndex(
                name: "IX_품질검사측정정보_회사코드_보유품목코드",
                table: "품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.CreateIndex(
                name: "IX_외주품질검사측정정보_회사코드_보유품목코드",
                table: "외주품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" });

            migrationBuilder.AddForeignKey(
                name: "FK_외주품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                table: "외주품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_품질검사측정정보_보유품목정보_회사코드_보유품목코드",
                table: "품질검사측정정보",
                columns: new[] { "회사코드", "보유품목코드" },
                principalTable: "보유품목정보",
                principalColumns: new[] { "회사코드", "보유품목코드" },
                onDelete: ReferentialAction.SetNull);
        }
    }
}
