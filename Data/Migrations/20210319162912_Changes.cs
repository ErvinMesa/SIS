using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SemesterName",
                table: "Semesters",
                type: "char(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SemesterName",
                table: "Semesters",
                type: "char(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(15)",
                oldNullable: true);
        }
    }
}
