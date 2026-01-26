using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_economy_api.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Frecuency",
                table: "FixedCosts",
                newName: "Frequency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Frequency",
                table: "FixedCosts",
                newName: "Frecuency");
        }
    }
}
