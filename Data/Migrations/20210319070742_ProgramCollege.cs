using Microsoft.EntityFrameworkCore.Migrations;

namespace SIS.Data.Migrations
{
    public partial class ProgramCollege : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    CollegeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollegeCode = table.Column<string>(type: "char(10)", nullable: true),
                    CollegeName = table.Column<string>(type: "varchar(50)", nullable: true),
                    NameofDean = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.CollegeID);
                });

            migrationBuilder.CreateTable(
                name: "EnrolledPrograms",
                columns: table => new
                {
                    ProgramID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramCode = table.Column<string>(type: "char(10)", nullable: true),
                    ProgramName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CollegeID = table.Column<int>(nullable: false),
                    NumberofSemester = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolledPrograms", x => x.ProgramID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colleges");

            migrationBuilder.DropTable(
                name: "EnrolledPrograms");
        }
    }
}
