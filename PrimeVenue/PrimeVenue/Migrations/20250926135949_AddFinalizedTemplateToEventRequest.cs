using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeVenue.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalizedTemplateToEventRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "TemplateVendors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalizedTemplateId",
                table: "EventRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRequests_FinalizedTemplateId",
                table: "EventRequests",
                column: "FinalizedTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventRequests_EventTemplates_FinalizedTemplateId",
                table: "EventRequests",
                column: "FinalizedTemplateId",
                principalTable: "EventTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventRequests_EventTemplates_FinalizedTemplateId",
                table: "EventRequests");

            migrationBuilder.DropIndex(
                name: "IX_EventRequests_FinalizedTemplateId",
                table: "EventRequests");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "TemplateVendors");

            migrationBuilder.DropColumn(
                name: "FinalizedTemplateId",
                table: "EventRequests");
        }
    }
}
