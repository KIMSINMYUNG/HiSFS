using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021061003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "외주위치코드",
                table: "외주생산위치정보",
                newName: "외주창고코드");

            migrationBuilder.AddColumn<string>(
                name: "외주장소코드",
                table: "외주생산위치정보",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "외주장소코드",
                table: "외주생산위치정보");

            migrationBuilder.RenameColumn(
                name: "외주창고코드",
                table: "외주생산위치정보",
                newName: "외주위치코드");
        }
    }
}
