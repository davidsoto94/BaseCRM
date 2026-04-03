using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseRMS.Migrations
{
    /// <inheritdoc />
    public partial class AddedClientFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                schema: "rms",
                table: "clients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                schema: "rms",
                table: "clients");
        }
    }
}
