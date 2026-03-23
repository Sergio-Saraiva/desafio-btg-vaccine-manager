using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueDocumentNowFilterDeleteAtAsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_DocumentType_DocumentNumber",
                table: "Persons");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DocumentType_DocumentNumber",
                table: "Persons",
                columns: new[] { "DocumentType", "DocumentNumber" },
                unique: true,
                filter: "[DeletedAt] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_DocumentType_DocumentNumber",
                table: "Persons");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DocumentType_DocumentNumber",
                table: "Persons",
                columns: new[] { "DocumentType", "DocumentNumber" },
                unique: true);
        }
    }
}
