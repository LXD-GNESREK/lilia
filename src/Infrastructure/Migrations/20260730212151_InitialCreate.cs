using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    DiscordID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    UnitCount_Units = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.DiscordID);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    InternalID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCode = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerDiscordID = table.Column<ulong>(type: "INTEGER", nullable: true),
                    ParentOrganisationID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.InternalID);
                    table.ForeignKey(
                        name: "FK_Organisations_Organisations_ParentOrganisationID",
                        column: x => x.ParentOrganisationID,
                        principalTable: "Organisations",
                        principalColumn: "InternalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organisations_Players_PlayerDiscordID",
                        column: x => x.PlayerDiscordID,
                        principalTable: "Players",
                        principalColumn: "DiscordID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    InternalID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OrganisationID = table.Column<Guid>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Modslots = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistryNumber = table.Column<string>(type: "TEXT", nullable: false),
                    UnitType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: true),
                    Width = table.Column<double>(type: "REAL", nullable: true),
                    Height = table.Column<double>(type: "REAL", nullable: true),
                    SpaceSpeed = table.Column<int>(type: "INTEGER", nullable: true),
                    AtmosphericSpeed = table.Column<int>(type: "INTEGER", nullable: true),
                    HyperdriveRating = table.Column<double>(type: "REAL", nullable: true),
                    Shields = table.Column<int>(type: "INTEGER", nullable: true),
                    Hull = table.Column<int>(type: "INTEGER", nullable: true),
                    Maneuverability = table.Column<int>(type: "INTEGER", nullable: true),
                    CargoCapacity = table.Column<int>(type: "INTEGER", nullable: true),
                    Consumables = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.InternalID);
                    table.ForeignKey(
                        name: "FK_Units_Organisations_OrganisationID",
                        column: x => x.OrganisationID,
                        principalTable: "Organisations",
                        principalColumn: "InternalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_ParentOrganisationID",
                table: "Organisations",
                column: "ParentOrganisationID");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_PlayerDiscordID_ShortCode",
                table: "Organisations",
                columns: new[] { "PlayerDiscordID", "ShortCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_OrganisationID",
                table: "Units",
                column: "OrganisationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
