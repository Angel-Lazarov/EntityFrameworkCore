using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P01_HospitalDatabase.Migrations
{
    public partial class DoctorIdChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Doctors",
                newName: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Doctors",
                newName: "Id");
        }
    }
}
