using Microsoft.EntityFrameworkCore.Migrations;

namespace HiSFS.Api.Host.Migrations
{
    public partial class _2021070201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "수량",
                table: "보유품목일지",
                type: "decimal(17,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "변경수량",
                table: "보유품목이력",
                type: "decimal(17,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "수량",
                table: "보유품목일지",
                type: "decimal(7,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "변경수량",
                table: "보유품목이력",
                type: "decimal(7,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,6)");
        }
    }
}
