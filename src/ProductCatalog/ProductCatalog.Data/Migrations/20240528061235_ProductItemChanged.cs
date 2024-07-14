using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class ProductItemChanged : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_items_brands_catalog_brand_id",
            table: "items");

        migrationBuilder.DropForeignKey(
            name: "fk_items_item_types_catalog_type_id",
            table: "items");

        migrationBuilder.DropColumn(
            name: "picture_file_name",
            table: "items");

        migrationBuilder.RenameColumn(
            name: "catalog_type_id",
            table: "items",
            newName: "product_type_id");

        migrationBuilder.RenameColumn(
            name: "catalog_brand_id",
            table: "items",
            newName: "product_brand_id");

        migrationBuilder.RenameIndex(
            name: "ix_items_catalog_type_id",
            table: "items",
            newName: "ix_items_product_type_id");

        migrationBuilder.RenameIndex(
            name: "ix_items_catalog_brand_id",
            table: "items",
            newName: "ix_items_product_brand_id");

        migrationBuilder.AddForeignKey(
            name: "fk_items_brands_product_brand_id",
            table: "items",
            column: "product_brand_id",
            principalTable: "brands",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_items_item_types_product_type_id",
            table: "items",
            column: "product_type_id",
            principalTable: "item_types",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_items_brands_product_brand_id",
            table: "items");

        migrationBuilder.DropForeignKey(
            name: "fk_items_item_types_product_type_id",
            table: "items");

        migrationBuilder.RenameColumn(
            name: "product_type_id",
            table: "items",
            newName: "catalog_type_id");

        migrationBuilder.RenameColumn(
            name: "product_brand_id",
            table: "items",
            newName: "catalog_brand_id");

        migrationBuilder.RenameIndex(
            name: "ix_items_product_type_id",
            table: "items",
            newName: "ix_items_catalog_type_id");

        migrationBuilder.RenameIndex(
            name: "ix_items_product_brand_id",
            table: "items",
            newName: "ix_items_catalog_brand_id");

        migrationBuilder.AddColumn<string>(
            name: "picture_file_name",
            table: "items",
            type: "text",
            nullable: true);

        migrationBuilder.AddForeignKey(
            name: "fk_items_brands_catalog_brand_id",
            table: "items",
            column: "catalog_brand_id",
            principalTable: "brands",
            principalColumn: "id");

        migrationBuilder.AddForeignKey(
            name: "fk_items_item_types_catalog_type_id",
            table: "items",
            column: "catalog_type_id",
            principalTable: "item_types",
            principalColumn: "id");
    }
}
