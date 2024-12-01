using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeviceStatusAndTypeToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Devices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Devices",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Devices",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Devices",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
