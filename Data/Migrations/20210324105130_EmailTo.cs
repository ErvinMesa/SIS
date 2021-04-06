using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class EmailTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailTo",
                table: "EmailTemplates",
                type: "varchar(150)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailTo",
                table: "EmailTemplates");
        }
    }
}
