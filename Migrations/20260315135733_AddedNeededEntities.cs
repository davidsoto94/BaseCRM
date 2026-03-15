using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BaseRMS.Migrations
{
    /// <inheritdoc />
    public partial class AddedNeededEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_logs",
                schema: "rms");

            migrationBuilder.CreateTable(
                name: "activity_logs",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trigger_user_email = table.Column<string>(type: "text", nullable: true),
                    affected_users_emails = table.Column<string[]>(type: "text[]", nullable: true),
                    activity_types = table.Column<string>(type: "text", nullable: false),
                    description_code = table.Column<string>(type: "text", nullable: true),
                    description_english = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activity_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "application_files",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    client_image_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contract_types",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees_contracts",
                schema: "rms",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    contract_id = table.Column<int>(type: "integer", nullable: false),
                    hourly_rate = table.Column<decimal>(type: "numeric", nullable: true),
                    daily_rate = table.Column<decimal>(type: "numeric", nullable: true),
                    contract_rate = table.Column<decimal>(type: "numeric", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees_contracts", x => new { x.employee_id, x.contract_id });
                });

            migrationBuilder.CreateTable(
                name: "event_categories",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "machine_types",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_machine_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "personal_identification_types",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personal_identification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "translations",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    field_name = table.Column<string>(type: "text", nullable: false),
                    language_code = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_translations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client_attachments",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_attachments_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_attachments_clients_client_id",
                        column: x => x.client_id,
                        principalSchema: "rms",
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_client_attachments_clients_entity_id",
                        column: x => x.ClientId,
                        principalSchema: "rms",
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    contract_id = table.Column<string>(type: "text", nullable: true),
                    contract_type_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    contract_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    hour_ammount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contracts", x => x.id);
                    table.ForeignKey(
                        name: "fk_contracts_clients_client_id",
                        column: x => x.client_id,
                        principalSchema: "rms",
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_contracts_contract_types_contract_type_id",
                        column: x => x.contract_type_id,
                        principalSchema: "rms",
                        principalTable: "contract_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "machines",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<string>(type: "text", nullable: false),
                    machine_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_machines", x => x.id);
                    table.ForeignKey(
                        name: "fk_machines_machine_types_machine_type_id",
                        column: x => x.machine_type_id,
                        principalSchema: "rms",
                        principalTable: "machine_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    personal_identification_string = table.Column<string>(type: "text", nullable: true),
                    personal_identification_type_id = table.Column<int>(type: "integer", nullable: false),
                    picture_path = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    emergency_contact_name = table.Column<string>(type: "text", nullable: false),
                    emergency_contact_phone = table.Column<string>(type: "text", nullable: false),
                    has_key = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_personal_identification_types_personal_identifica",
                        column: x => x.personal_identification_type_id,
                        principalSchema: "rms",
                        principalTable: "personal_identification_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contract_attachments",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_contract_attachments_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_contract_attachments_contracts_entity_id",
                        column: x => x.entity_id,
                        principalSchema: "rms",
                        principalTable: "contracts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "machine_attachments",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_machine_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_machine_attachments_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_machine_attachments_machines_entity_id",
                        column: x => x.entity_id,
                        principalSchema: "rms",
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_attachments",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_employee_attachments_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_employee_attachments_employees_entity_id",
                        column: x => x.entity_id,
                        principalSchema: "rms",
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contract_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    machine_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observations = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_events_contracts_contract_id",
                        column: x => x.contract_id,
                        principalSchema: "rms",
                        principalTable: "contracts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_events_employees_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "rms",
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_events_machines_machine_id",
                        column: x => x.machine_id,
                        principalSchema: "rms",
                        principalTable: "machines",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_attachments",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_id = table.Column<int>(type: "integer", nullable: false),
                    file_id = table.Column<int>(type: "integer", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_attachments_application_files_file_id",
                        column: x => x.file_id,
                        principalSchema: "rms",
                        principalTable: "application_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_attachments_events_entity_id",
                        column: x => x.entity_id,
                        principalSchema: "rms",
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_client_attachments_client_id",
                schema: "rms",
                table: "client_attachments",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_attachments_entity_id",
                schema: "rms",
                table: "client_attachments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "ix_client_attachments_file_id",
                schema: "rms",
                table: "client_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_attachments_entity_id",
                schema: "rms",
                table: "contract_attachments",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_attachments_file_id",
                schema: "rms",
                table: "contract_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_contracts_client_id",
                schema: "rms",
                table: "contracts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_contracts_contract_type_id",
                schema: "rms",
                table: "contracts",
                column: "contract_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_attachments_entity_id",
                schema: "rms",
                table: "employee_attachments",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_attachments_file_id",
                schema: "rms",
                table: "employee_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_personal_identification_type_id",
                schema: "rms",
                table: "employees",
                column: "personal_identification_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_attachments_entity_id",
                schema: "rms",
                table: "event_attachments",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_attachments_file_id",
                schema: "rms",
                table: "event_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_contract_id",
                schema: "rms",
                table: "events",
                column: "contract_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_employee_id",
                schema: "rms",
                table: "events",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_machine_id",
                schema: "rms",
                table: "events",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "ix_machine_attachments_entity_id",
                schema: "rms",
                table: "machine_attachments",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_machine_attachments_file_id",
                schema: "rms",
                table: "machine_attachments",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_machines_machine_type_id",
                schema: "rms",
                table: "machines",
                column: "machine_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_logs",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "client_attachments",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "contract_attachments",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "employee_attachments",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "employees_contracts",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "event_attachments",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "event_categories",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "machine_attachments",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "translations",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "events",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "application_files",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "contracts",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "employees",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "machines",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "clients",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "contract_types",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "personal_identification_types",
                schema: "rms");

            migrationBuilder.DropTable(
                name: "machine_types",
                schema: "rms");

            migrationBuilder.CreateTable(
                name: "event_logs",
                schema: "rms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    affected_users_emails = table.Column<string[]>(type: "text[]", nullable: true),
                    description_code = table.Column<string>(type: "text", nullable: true),
                    description_english = table.Column<string>(type: "text", nullable: true),
                    event_types = table.Column<string>(type: "text", nullable: false),
                    trigger_user_email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_logs", x => x.id);
                });
        }
    }
}
