using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class SoftDeleting : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "orders",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "order_items",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "catalog_types",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "catalog_brands",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "Catalog",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "baskets",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "is_deleted",
            table: "basket_items",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "orders");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "order_items");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "catalog_types");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "catalog_brands");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "Catalog");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "baskets");

        migrationBuilder.DropColumn(
            name: "is_deleted",
            table: "basket_items");
    }
}
