using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Appointmentupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointments",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                newName: "IX_Appointments_DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Departments_DepartmentId",
                table: "Appointments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Departments_DepartmentId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Appointments",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DepartmentId",
                table: "Appointments",
                newName: "IX_Appointments_PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
