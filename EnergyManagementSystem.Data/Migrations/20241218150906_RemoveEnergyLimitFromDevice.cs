using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEnergyLimitFromDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyLimit",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LimitType",
                table: "Devices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EnergyLimit",
                table: "Devices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LimitType",
                table: "Devices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
