using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddsUnitConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitConversion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceUnitOfMeasureId = table.Column<int>(type: "int", nullable: false),
                    DestinationUnitOfMeasureId = table.Column<int>(type: "int", nullable: false),
                    ConversionRatio = table.Column<decimal>(type: "decimal(9,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConversion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitConversion_UnitOfMeasure_DestinationUnitOfMeasureId",
                        column: x => x.DestinationUnitOfMeasureId,
                        principalTable: "UnitOfMeasure",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitConversion_UnitOfMeasure_SourceUnitOfMeasureId",
                        column: x => x.SourceUnitOfMeasureId,
                        principalTable: "UnitOfMeasure",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversion_DestinationUnitOfMeasureId",
                table: "UnitConversion",
                column: "DestinationUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversion_SourceUnitOfMeasureId_DestinationUnitOfMeasureId",
                table: "UnitConversion",
                columns: new[] { "SourceUnitOfMeasureId", "DestinationUnitOfMeasureId" },
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitConversion");
        }
    }
}
