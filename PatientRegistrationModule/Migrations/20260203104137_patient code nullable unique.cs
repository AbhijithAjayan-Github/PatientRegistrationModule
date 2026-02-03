using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRegistrationModule.Migrations
{
    /// <inheritdoc />
    public partial class patientcodenullableunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientCode",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PatientCode",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCode",
                table: "Patients",
                column: "PatientCode",
                unique: true,
                filter: "[PatientCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientCode",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PatientCode",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCode",
                table: "Patients",
                column: "PatientCode",
                unique: true);
        }
    }
}
