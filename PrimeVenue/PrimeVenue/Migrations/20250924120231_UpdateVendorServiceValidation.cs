using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeVenue.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVendorServiceValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "VendorServices",
                type: "float",
                nullable: true,
                defaultValue: 1.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceEstimate",
                table: "VendorServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 1000m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "VendorServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 1.0);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceEstimate",
                table: "VendorServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 1000m);
        }
    }
}
