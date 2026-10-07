using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutsStats.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEventIdVenueUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_EventId_EventUrl",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventId_VenueEntityId",
                table: "Events",
                columns: new[] { "EventId", "VenueEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventUrl",
                table: "Events",
                column: "EventUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_EventId_VenueEntityId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventUrl",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventId_EventUrl",
                table: "Events",
                columns: new[] { "EventId", "EventUrl" });
        }
    }
}

