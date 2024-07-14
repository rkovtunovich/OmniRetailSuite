using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence<int>(
            name: "cashier_codes");

        migrationBuilder.CreateSequence<int>(
            name: "receipt_codes");

        migrationBuilder.CreateSequence<int>(
            name: "store_codes");

        migrationBuilder.CreateTable(
            name: "cashiers",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                code_number = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"cashier_codes\"')"),
                code_prefix = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cashiers", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "product_items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                code_number = table.Column<int>(type: "integer", nullable: false),
                code_prefix = table.Column<string>(type: "text", nullable: true),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_product_items", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "stores",
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
                table.PrimaryKey("pk_stores", x => x.id);
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
                    name: "fk_cashier_store_stores_store_id",
                    column: x => x.store_id,
                    principalTable: "stores",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "receipts",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                code_number = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"receipt_codes\"')"),
                code_prefix = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                store_id = table.Column<Guid>(type: "uuid", nullable: false),
                cashier_id = table.Column<Guid>(type: "uuid", nullable: false),
                total_price = table.Column<decimal>(type: "numeric(18)", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_receipts", x => x.id);
                table.ForeignKey(
                    name: "fk_receipts_cashiers_cashier_id",
                    column: x => x.cashier_id,
                    principalTable: "cashiers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_receipts_stores_store_id",
                    column: x => x.store_id,
                    principalTable: "stores",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "receipt_items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                receipt_id = table.Column<Guid>(type: "uuid", nullable: false),
                line_number = table.Column<int>(type: "integer", nullable: false),
                product_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                quantity = table.Column<double>(type: "numeric(18,3)", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric(18)", nullable: false),
                total_price = table.Column<decimal>(type: "numeric(18)", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_receipt_items", x => x.id);
                table.ForeignKey(
                    name: "fk_receipt_items_product_items_product_item_id",
                    column: x => x.product_item_id,
                    principalTable: "product_items",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_receipt_items_receipts_receipt_id",
                    column: x => x.receipt_id,
                    principalTable: "receipts",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_cashier_store_store_id",
            table: "cashier_store",
            column: "store_id");

        migrationBuilder.CreateIndex(
            name: "ix_cashiers_code_prefix_code_number",
            table: "cashiers",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_receipt_items_product_item_id",
            table: "receipt_items",
            column: "product_item_id");

        migrationBuilder.CreateIndex(
            name: "ix_receipt_items_receipt_id",
            table: "receipt_items",
            column: "receipt_id");

        migrationBuilder.CreateIndex(
            name: "ix_receipts_cashier_id",
            table: "receipts",
            column: "cashier_id");

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
            name: "ix_stores_code_prefix_code_number",
            table: "stores",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "cashier_store");

        migrationBuilder.DropTable(
            name: "receipt_items");

        migrationBuilder.DropTable(
            name: "product_items");

        migrationBuilder.DropTable(
            name: "receipts");

        migrationBuilder.DropTable(
            name: "cashiers");

        migrationBuilder.DropTable(
            name: "stores");

        migrationBuilder.DropSequence(
            name: "cashier_codes");

        migrationBuilder.DropSequence(
            name: "receipt_codes");

        migrationBuilder.DropSequence(
            name: "store_codes");
    }
}
