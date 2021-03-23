using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class BirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "StudentProfiles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "StudentProfiles",
                type: "char(10)",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
