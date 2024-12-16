using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLimitTypeToDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LimitType",
                table: "Devices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitType",
                table: "Devices");
        }
    }
}
