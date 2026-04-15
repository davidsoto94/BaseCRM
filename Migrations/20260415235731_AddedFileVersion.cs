using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BaseRMS.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                schema: "rms",
                table: "application_files");

            migrationBuilder.AddColumn<int>(
                name: "current_version_id",
                schema: "rms",
                table: "application_files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "file_versions",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    storage_path = table.Column<string>(type: "text", nullable: false),
                    version_number = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_versions", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_versions_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_files_current_version_id",
                schema: "rms",
                table: "application_files",
                column: "current_version_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_versions_file_id",
                schema: "rms",
                table: "file_versions",
                column: "file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_application_files_file_versions_current_version_id",
                schema: "rms",
                table: "application_files",
                column: "current_version_id",
                principalSchema: "rms",
                principalTable: "file_versions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_files_file_versions_current_version_id",
                schema: "rms",
                table: "application_files");

            migrationBuilder.DropTable(
                name: "file_versions",
                schema: "rms");

            migrationBuilder.DropIndex(
                name: "ix_application_files_current_version_id",
                schema: "rms",
                table: "application_files");

            migrationBuilder.DropColumn(
                name: "current_version_id",
                schema: "rms",
                table: "application_files");

            migrationBuilder.AddColumn<string>(
                name: "path",
                schema: "rms",
                table: "application_files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
