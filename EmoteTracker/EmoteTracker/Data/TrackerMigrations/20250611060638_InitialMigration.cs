using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmoteTracker.Data.TrackerMigrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitchChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitchChannelEmotes",
                columns: table => new
                {
                    EmoteId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TwitchChannelId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CanonicalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    IsListed = table.Column<bool>(type: "bit", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmoteType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchChannelEmotes", x => new { x.EmoteId, x.TwitchChannelId });
                    table.ForeignKey(
                        name: "FK_TwitchChannelEmotes_TwitchChannels_TwitchChannelId",
                        column: x => x.TwitchChannelId,
                        principalTable: "TwitchChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchChannelEmotes_TwitchChannelId",
                table: "TwitchChannelEmotes",
                column: "TwitchChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitchChannelEmotes");

            migrationBuilder.DropTable(
                name: "TwitchChannels");
        }
    }
}
