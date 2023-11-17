using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations;

/// <inheritdoc />
public partial class RenamedItem : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_receipt_items_catalog_items_catalog_item_id",
            table: "receipt_items");

        migrationBuilder.DropTable(
            name: "catalog_items");

        migrationBuilder.RenameColumn(
            name: "catalog_item_id",
            table: "receipt_items",
            newName: "product_item_id");

        migrationBuilder.RenameIndex(
            name: "ix_receipt_items_catalog_item_id",
            table: "receipt_items",
            newName: "ix_receipt_items_product_item_id");

        migrationBuilder.CreateTable(
            name: "product_items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_product_items", x => x.id);
            });

        migrationBuilder.AddForeignKey(
            name: "fk_receipt_items_product_items_product_item_id",
            table: "receipt_items",
            column: "product_item_id",
            principalTable: "product_items",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_receipt_items_product_items_product_item_id",
            table: "receipt_items");

        migrationBuilder.DropTable(
            name: "product_items");

        migrationBuilder.RenameColumn(
            name: "product_item_id",
            table: "receipt_items",
            newName: "catalog_item_id");

        migrationBuilder.RenameIndex(
            name: "ix_receipt_items_product_item_id",
            table: "receipt_items",
            newName: "ix_receipt_items_catalog_item_id");

        migrationBuilder.CreateTable(
            name: "catalog_items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog_items", x => x.id);
            });

        migrationBuilder.AddForeignKey(
            name: "fk_receipt_items_catalog_items_catalog_item_id",
            table: "receipt_items",
            column: "catalog_item_id",
            principalTable: "catalog_items",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
