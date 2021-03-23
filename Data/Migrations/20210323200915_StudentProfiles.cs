using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class StudentProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "StudentProfiles",
                type: "char(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileStamp",
                table: "StudentProfiles",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StudentProfiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "StudentProfiles",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileStamp",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "StudentProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "StudentProfiles",
                type: "char(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)");
        }
    }
}
