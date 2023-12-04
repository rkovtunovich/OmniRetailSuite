using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations;

/// <inheritdoc />
public partial class Store : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "number",
            table: "receipts");

        migrationBuilder.CreateSequence<int>(
            name: "cashier_codes");

        migrationBuilder.CreateSequence<int>(
            name: "receipt_codes");

        migrationBuilder.CreateSequence<int>(
            name: "store_codes");

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "receipts",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"receipt_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "receipts",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "store_id",
            table: "receipts",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "product_items",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "product_items",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "cashiers",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"cashier_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "cashiers",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.CreateTable(
            name: "store",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                code_number = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"store_codes\"')"),
                code_prefix = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_store", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "cashier_store",
            columns: table => new
            {
                cashiers_id = table.Column<Guid>(type: "uuid", nullable: false),
                store_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cashier_store", x => new { x.cashiers_id, x.store_id });
                table.ForeignKey(
                    name: "fk_cashier_store_cashiers_cashiers_id",
                    column: x => x.cashiers_id,
                    principalTable: "cashiers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_cashier_store_store_store_id",
                    column: x => x.store_id,
                    principalTable: "store",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_receipts_code_prefix_code_number",
            table: "receipts",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_receipts_store_id",
            table: "receipts",
            column: "store_id");

        migrationBuilder.CreateIndex(
            name: "ix_cashiers_code_prefix_code_number",
            table: "cashiers",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_cashier_store_store_id",
            table: "cashier_store",
            column: "store_id");

        migrationBuilder.CreateIndex(
            name: "ix_store_code_prefix_code_number",
            table: "store",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "fk_receipts_store_store_id",
            table: "receipts",
            column: "store_id",
            principalTable: "store",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_receipts_store_store_id",
            table: "receipts");

        migrationBuilder.DropTable(
            name: "cashier_store");

        migrationBuilder.DropTable(
            name: "store");

        migrationBuilder.DropIndex(
            name: "ix_receipts_code_prefix_code_number",
            table: "receipts");

        migrationBuilder.DropIndex(
            name: "ix_receipts_store_id",
            table: "receipts");

        migrationBuilder.DropIndex(
            name: "ix_cashiers_code_prefix_code_number",
            table: "cashiers");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "receipts");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "receipts");

        migrationBuilder.DropColumn(
            name: "store_id",
            table: "receipts");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "product_items");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "product_items");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "cashiers");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "cashiers");

        migrationBuilder.DropSequence(
            name: "cashier_codes");

        migrationBuilder.DropSequence(
            name: "receipt_codes");

        migrationBuilder.DropSequence(
            name: "store_codes");

        migrationBuilder.AddColumn<string>(
            name: "number",
            table: "receipts",
            type: "character varying(9)",
            maxLength: 9,
            nullable: false,
            defaultValue: "");
    }
}
