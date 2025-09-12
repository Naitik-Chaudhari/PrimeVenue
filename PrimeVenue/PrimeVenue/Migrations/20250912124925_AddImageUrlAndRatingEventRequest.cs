using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeVenue.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlAndRatingEventRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOrganized",
                table: "EventRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "EventRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedServices",
                table: "EventRequests",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "IsOrganized",
                table: "EventRequests");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "EventRequests");

            migrationBuilder.DropColumn(
                name: "RequestedServices",
                table: "EventRequests");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");
        }
    }
}
