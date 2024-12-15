using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataColumnToUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "UserTokens",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "UserTokens");
        }
    }
}
