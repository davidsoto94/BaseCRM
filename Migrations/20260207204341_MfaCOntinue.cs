using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseCRM.Migrations
{
    /// <inheritdoc />
    public partial class MfaCOntinue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trusted_device_application_user_user_id",
                schema: "crm",
                table: "trusted_device");

            migrationBuilder.DropPrimaryKey(
                name: "pk_trusted_device",
                schema: "crm",
                table: "trusted_device");

            migrationBuilder.RenameTable(
                name: "trusted_device",
                schema: "crm",
                newName: "trusted_devices",
                newSchema: "crm");

            migrationBuilder.RenameIndex(
                name: "ix_trusted_device_user_id",
                schema: "crm",
                table: "trusted_devices",
                newName: "ix_trusted_devices_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_trusted_devices",
                schema: "crm",
                table: "trusted_devices",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_trusted_devices_application_user_user_id",
                schema: "crm",
                table: "trusted_devices",
                column: "user_id",
                principalSchema: "crm",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trusted_devices_application_user_user_id",
                schema: "crm",
                table: "trusted_devices");

            migrationBuilder.DropPrimaryKey(
                name: "pk_trusted_devices",
                schema: "crm",
                table: "trusted_devices");

            migrationBuilder.RenameTable(
                name: "trusted_devices",
                schema: "crm",
                newName: "trusted_device",
                newSchema: "crm");

            migrationBuilder.RenameIndex(
                name: "ix_trusted_devices_user_id",
                schema: "crm",
                table: "trusted_device",
                newName: "ix_trusted_device_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_trusted_device",
                schema: "crm",
                table: "trusted_device",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_trusted_device_application_user_user_id",
                schema: "crm",
                table: "trusted_device",
                column: "user_id",
                principalSchema: "crm",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
