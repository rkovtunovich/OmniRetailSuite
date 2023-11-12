using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitialPostgres : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence(
            name: "catalog_brand_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_type_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "baskets",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                buyer_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_baskets", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "catalog_brands",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog_brands", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "catalog_types",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "orders",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                buyer_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                order_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ship_to_address_street = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                ship_to_address_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                ship_to_address_state = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                ship_to_address_country = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false),
                ship_to_address_zip_code = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_orders", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "basket_items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                quantity = table.Column<int>(type: "integer", nullable: false),
                catalog_item_id = table.Column<int>(type: "integer", nullable: false),
                basket_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_basket_items", x => x.id);
                table.ForeignKey(
                    name: "fk_basket_items_baskets_basket_id",
                    column: x => x.basket_id,
                    principalTable: "baskets",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Catalog",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                picture_uri = table.Column<string>(type: "text", nullable: true),
                catalog_type_id = table.Column<int>(type: "integer", nullable: false),
                catalog_brand_id = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog", x => x.id);
                table.ForeignKey(
                    name: "fk_catalog_catalog_brands_catalog_brand_id",
                    column: x => x.catalog_brand_id,
                    principalTable: "catalog_brands",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_catalog_catalog_types_catalog_type_id",
                    column: x => x.catalog_type_id,
                    principalTable: "catalog_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "order_items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                item_ordered_catalog_item_id = table.Column<int>(type: "integer", nullable: false),
                item_ordered_product_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                item_ordered_picture_uri = table.Column<string>(type: "text", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                units = table.Column<int>(type: "integer", nullable: false),
                order_id = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_order_items", x => x.id);
                table.ForeignKey(
                    name: "fk_order_items_orders_order_id",
                    column: x => x.order_id,
                    principalTable: "orders",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_basket_items_basket_id",
            table: "basket_items",
            column: "basket_id");

        migrationBuilder.CreateIndex(
            name: "ix_catalog_catalog_brand_id",
            table: "Catalog",
            column: "catalog_brand_id");

        migrationBuilder.CreateIndex(
            name: "ix_catalog_catalog_type_id",
            table: "Catalog",
            column: "catalog_type_id");

        migrationBuilder.CreateIndex(
            name: "ix_order_items_order_id",
            table: "order_items",
            column: "order_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "basket_items");

        migrationBuilder.DropTable(
            name: "Catalog");

        migrationBuilder.DropTable(
            name: "order_items");

        migrationBuilder.DropTable(
            name: "baskets");

        migrationBuilder.DropTable(
            name: "catalog_brands");

        migrationBuilder.DropTable(
            name: "catalog_types");

        migrationBuilder.DropTable(
            name: "orders");

        migrationBuilder.DropSequence(
            name: "catalog_brand_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_type_hilo");
    }
}
