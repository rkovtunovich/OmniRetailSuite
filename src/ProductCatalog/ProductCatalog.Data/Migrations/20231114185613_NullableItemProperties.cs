using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class NullableItemProperties : Migration
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

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_type_id",
            table: "items",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_brand_id",
            table: "items",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid");

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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_items_brands_catalog_brand_id",
            table: "items");

        migrationBuilder.DropForeignKey(
            name: "fk_items_item_types_catalog_type_id",
            table: "items");

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_type_id",
            table: "items",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_brand_id",
            table: "items",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "fk_items_brands_catalog_brand_id",
            table: "items",
            column: "catalog_brand_id",
            principalTable: "brands",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_items_item_types_catalog_type_id",
            table: "items",
            column: "catalog_type_id",
            principalTable: "item_types",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
