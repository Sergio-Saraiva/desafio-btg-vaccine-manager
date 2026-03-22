using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovingColumnDoseNumberFromVaccinationRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoseNumber",
                table: "VaccinationRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoseNumber",
                table: "VaccinationRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
