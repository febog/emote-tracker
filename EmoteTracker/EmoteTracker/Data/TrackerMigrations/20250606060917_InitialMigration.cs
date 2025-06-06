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
                name: "EmoteServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmoteServices", x => x.Id);
                });

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
                name: "Emotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EmoteServiceId = table.Column<int>(type: "int", nullable: false),
                    CanonicalName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    IsListed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emotes_EmoteServices_EmoteServiceId",
                        column: x => x.EmoteServiceId,
                        principalTable: "EmoteServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TwitchChannelEmotes",
                columns: table => new
                {
                    EmoteId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TwitchChannelId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchChannelEmotes", x => new { x.EmoteId, x.TwitchChannelId });
                    table.ForeignKey(
                        name: "FK_TwitchChannelEmotes_Emotes_EmoteId",
                        column: x => x.EmoteId,
                        principalTable: "Emotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitchChannelEmotes_TwitchChannels_TwitchChannelId",
                        column: x => x.TwitchChannelId,
                        principalTable: "TwitchChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emotes_EmoteServiceId",
                table: "Emotes",
                column: "EmoteServiceId");

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
                name: "Emotes");

            migrationBuilder.DropTable(
                name: "TwitchChannels");

            migrationBuilder.DropTable(
                name: "EmoteServices");
        }
    }
}
