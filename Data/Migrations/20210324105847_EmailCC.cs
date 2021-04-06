using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class EmailCC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailCC",
                table: "EmailTemplates",
                type: "varchar(150)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailCC",
                table: "EmailTemplates");
        }
    }
}
