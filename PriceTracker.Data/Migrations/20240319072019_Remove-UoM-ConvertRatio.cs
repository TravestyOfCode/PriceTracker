using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUoMConvertRatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionToGramsRatio",
                table: "UnitOfMeasure");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConversionToGramsRatio",
                table: "UnitOfMeasure",
                type: "decimal(9,5)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
