using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Completed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DidTeam1Win = table.Column<bool>(type: "boolean", nullable: false),
                    Team1Members = table.Column<string>(type: "text", nullable: false),
                    Team2Members = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Losses = table.Column<int>(type: "integer", nullable: false),
                    Elo = table.Column<int>(type: "integer", nullable: false),
                    HighestElo = table.Column<int>(type: "integer", nullable: false),
                    LongestWinstreak = table.Column<int>(type: "integer", nullable: false),
                    LongestLossstreak = table.Column<int>(type: "integer", nullable: false),
                    CurrentWinstreak = table.Column<int>(type: "integer", nullable: false),
                    CurrentLossstreak = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchResults");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
