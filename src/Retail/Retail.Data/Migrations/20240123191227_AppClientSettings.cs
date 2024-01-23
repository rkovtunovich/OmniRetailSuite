using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations;

/// <inheritdoc />
public partial class AppClientSettings : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "app_client_settings",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                store_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_app_client_settings", x => x.id);
                table.ForeignKey(
                    name: "fk_app_client_settings_stores_store_id",
                    column: x => x.store_id,
                    principalTable: "stores",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_app_client_settings_store_id",
            table: "app_client_settings",
            column: "store_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "app_client_settings");
    }
}
