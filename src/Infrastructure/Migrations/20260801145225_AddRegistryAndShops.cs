using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistryAndShops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtmosphericSpeed",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CargoCapacity",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Consumables",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Hull",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "HyperdriveRating",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Maneuverability",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Modslots",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Shields",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "SpaceSpeed",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Units",
                newName: "RegistryUnitID");

            migrationBuilder.CreateTable(
                name: "RegistryUnits",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<long>(type: "INTEGER", nullable: false),
                    Modslots = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    HasStats = table.Column<bool>(type: "INTEGER", nullable: false),
                    StatDataJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistryUnits", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ShopRegistryUnits",
                columns: table => new
                {
                    ShopID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RegistryUnitID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopRegistryUnits", x => new { x.ShopID, x.RegistryUnitID });
                    table.ForeignKey(
                        name: "FK_ShopRegistryUnits_RegistryUnits_RegistryUnitID",
                        column: x => x.RegistryUnitID,
                        principalTable: "RegistryUnits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopRegistryUnits_Shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "Shops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_RegistryUnitID",
                table: "Units",
                column: "RegistryUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_ShopRegistryUnits_RegistryUnitID",
                table: "ShopRegistryUnits",
                column: "RegistryUnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_RegistryUnits_RegistryUnitID",
                table: "Units",
                column: "RegistryUnitID",
                principalTable: "RegistryUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_RegistryUnits_RegistryUnitID",
                table: "Units");

            migrationBuilder.DropTable(
                name: "ShopRegistryUnits");

            migrationBuilder.DropTable(
                name: "RegistryUnits");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Units_RegistryUnitID",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "RegistryUnitID",
                table: "Units",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "AtmosphericSpeed",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoCapacity",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Consumables",
                table: "Units",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Units",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Hull",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HyperdriveRating",
                table: "Units",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Length",
                table: "Units",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Maneuverability",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Modslots",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Units",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shields",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpaceSpeed",
                table: "Units",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Width",
                table: "Units",
                type: "REAL",
                nullable: true);
        }
    }
}
