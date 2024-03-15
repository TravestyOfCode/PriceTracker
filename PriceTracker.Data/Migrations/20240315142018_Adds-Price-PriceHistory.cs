using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddsPricePriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "PriceHistory",
                type: "decimal(9,5)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "PriceHistory");
        }
    }
}
