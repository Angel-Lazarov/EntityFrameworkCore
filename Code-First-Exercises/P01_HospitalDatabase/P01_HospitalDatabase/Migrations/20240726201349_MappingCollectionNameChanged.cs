using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P01_HospitalDatabase.Migrations
{
    public partial class MappingCollectionNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnose_Patient_PatientId",
                table: "Diagnose");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicament_Medicament_MedicamentId",
                table: "PatientMedicament");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicament_Patient_PatientId",
                table: "PatientMedicament");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitation_Patient_PatientId",
                table: "Visitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visitation",
                table: "Visitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientMedicament",
                table: "PatientMedicament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patient",
                table: "Patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicament",
                table: "Medicament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diagnose",
                table: "Diagnose");

            migrationBuilder.RenameTable(
                name: "Visitation",
                newName: "Visitations");

            migrationBuilder.RenameTable(
                name: "PatientMedicament",
                newName: "Prescriptions");

            migrationBuilder.RenameTable(
                name: "Patient",
                newName: "Patients");

            migrationBuilder.RenameTable(
                name: "Medicament",
                newName: "Medicaments");

            migrationBuilder.RenameTable(
                name: "Diagnose",
                newName: "Diagnoses");

            migrationBuilder.RenameIndex(
                name: "IX_Visitation_PatientId",
                table: "Visitations",
                newName: "IX_Visitations_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientMedicament_MedicamentId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_MedicamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagnose_PatientId",
                table: "Diagnoses",
                newName: "IX_Diagnoses_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visitations",
                table: "Visitations",
                column: "VisitationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prescriptions",
                table: "Prescriptions",
                columns: new[] { "PatientId", "MedicamentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicaments",
                table: "Medicaments",
                column: "MedicamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses",
                column: "DiagnoseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Patients_PatientId",
                table: "Diagnoses",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Medicaments_MedicamentId",
                table: "Prescriptions",
                column: "MedicamentId",
                principalTable: "Medicaments",
                principalColumn: "MedicamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Patients_PatientId",
                table: "Visitations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Patients_PatientId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Medicaments_MedicamentId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Patients_PatientId",
                table: "Visitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visitations",
                table: "Visitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prescriptions",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicaments",
                table: "Medicaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diagnoses",
                table: "Diagnoses");

            migrationBuilder.RenameTable(
                name: "Visitations",
                newName: "Visitation");

            migrationBuilder.RenameTable(
                name: "Prescriptions",
                newName: "PatientMedicament");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patient");

            migrationBuilder.RenameTable(
                name: "Medicaments",
                newName: "Medicament");

            migrationBuilder.RenameTable(
                name: "Diagnoses",
                newName: "Diagnose");

            migrationBuilder.RenameIndex(
                name: "IX_Visitations_PatientId",
                table: "Visitation",
                newName: "IX_Visitation_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_MedicamentId",
                table: "PatientMedicament",
                newName: "IX_PatientMedicament_MedicamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Diagnoses_PatientId",
                table: "Diagnose",
                newName: "IX_Diagnose_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visitation",
                table: "Visitation",
                column: "VisitationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientMedicament",
                table: "PatientMedicament",
                columns: new[] { "PatientId", "MedicamentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patient",
                table: "Patient",
                column: "PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicament",
                table: "Medicament",
                column: "MedicamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diagnose",
                table: "Diagnose",
                column: "DiagnoseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnose_Patient_PatientId",
                table: "Diagnose",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicament_Medicament_MedicamentId",
                table: "PatientMedicament",
                column: "MedicamentId",
                principalTable: "Medicament",
                principalColumn: "MedicamentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicament_Patient_PatientId",
                table: "PatientMedicament",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitation_Patient_PatientId",
                table: "Visitation",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
