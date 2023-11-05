using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retail.Api.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence(
            name: "catalog_item_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "receipt_item_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "cashiers",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cashiers", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "catalog_items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog_items", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "receipts",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                receipt_number = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                receipt_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                cashier_id = table.Column<int>(type: "integer", nullable: false),
                total_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
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
            });

        migrationBuilder.CreateTable(
            name: "receipt_items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                receipt_id = table.Column<int>(type: "integer", nullable: false),
                number = table.Column<int>(type: "integer", nullable: false),
                catalog_item_id = table.Column<int>(type: "integer", nullable: false),
                quantity = table.Column<double>(type: "numeric(18,3)", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                total_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_receipt_items", x => x.id);
                table.ForeignKey(
                    name: "fk_receipt_items_catalog_items_catalog_item_id",
                    column: x => x.catalog_item_id,
                    principalTable: "catalog_items",
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
            name: "ix_receipt_items_catalog_item_id",
            table: "receipt_items",
            column: "catalog_item_id");

        migrationBuilder.CreateIndex(
            name: "ix_receipt_items_receipt_id",
            table: "receipt_items",
            column: "receipt_id");

        migrationBuilder.CreateIndex(
            name: "ix_receipts_cashier_id",
            table: "receipts",
            column: "cashier_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "receipt_items");

        migrationBuilder.DropTable(
            name: "catalog_items");

        migrationBuilder.DropTable(
            name: "receipts");

        migrationBuilder.DropTable(
            name: "cashiers");

        migrationBuilder.DropSequence(
            name: "catalog_item_hilo");

        migrationBuilder.DropSequence(
            name: "receipt_item_hilo");
    }
}
