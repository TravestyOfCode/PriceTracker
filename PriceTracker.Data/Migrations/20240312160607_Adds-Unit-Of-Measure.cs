using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Data.Migrations;

/// <inheritdoc />
internal partial class AddsUnitOfMeasure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UnitOfMeasure",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                Abbreviation = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                ConversionToGramsRatio = table.Column<decimal>(type: "decimal(9,5)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UnitOfMeasure", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UnitOfMeasure");
    }
}
