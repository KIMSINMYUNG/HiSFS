using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021053003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_외주지시별품질검사장비_외주지시별검사정보_지시번호_품질검사코드",
                table: "외주지시별품질검사장비");

            migrationBuilder.DropPrimaryKey(
                name: "PK_외주지시별품질검사장비",
                table: "외주지시별품질검사장비");

            migrationBuilder.AlterColumn<string>(
                name: "품질검사코드",
                table: "외주지시별품질검사장비",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_외주지시별품질검사장비",
                table: "외주지시별품질검사장비",
                columns: new[] { "지시번호", "검사장비식별번호" });

            migrationBuilder.CreateIndex(
                name: "IX_외주지시별품질검사장비_지시번호_품질검사코드",
                table: "외주지시별품질검사장비",
                columns: new[] { "지시번호", "품질검사코드" });

            migrationBuilder.AddForeignKey(
                name: "FK_외주지시별품질검사장비_외주지시별검사정보_지시번호_품질검사코드",
                table: "외주지시별품질검사장비",
                columns: new[] { "지시번호", "품질검사코드" },
                principalTable: "외주지시별검사정보",
                principalColumns: new[] { "지시번호", "품질검사코드" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_외주지시별품질검사장비_외주지시별검사정보_지시번호_품질검사코드",
                table: "외주지시별품질검사장비");

            migrationBuilder.DropPrimaryKey(
                name: "PK_외주지시별품질검사장비",
                table: "외주지시별품질검사장비");

            migrationBuilder.DropIndex(
                name: "IX_외주지시별품질검사장비_지시번호_품질검사코드",
                table: "외주지시별품질검사장비");

            migrationBuilder.AlterColumn<string>(
                name: "품질검사코드",
                table: "외주지시별품질검사장비",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_외주지시별품질검사장비",
                table: "외주지시별품질검사장비",
                columns: new[] { "지시번호", "품질검사코드", "검사장비식별번호" });

            migrationBuilder.AddForeignKey(
                name: "FK_외주지시별품질검사장비_외주지시별검사정보_지시번호_품질검사코드",
                table: "외주지시별품질검사장비",
                columns: new[] { "지시번호", "품질검사코드" },
                principalTable: "외주지시별검사정보",
                principalColumns: new[] { "지시번호", "품질검사코드" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
