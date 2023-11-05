using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Refactoring : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Catalog");

        migrationBuilder.DropTable(
            name: "catalog_brands");

        migrationBuilder.DropTable(
            name: "catalog_types");

        migrationBuilder.DropSequence(
            name: "catalog_brand_hilo");

        migrationBuilder.CreateSequence(
            name: "brand_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "brands",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_brands", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "item_types",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_item_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                picture_file_name = table.Column<string>(type: "text", nullable: true),
                picture_uri = table.Column<string>(type: "text", nullable: true),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                catalog_type_id = table.Column<int>(type: "integer", nullable: false),
                barcode = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                catalog_brand_id = table.Column<int>(type: "integer", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_items", x => x.id);
                table.ForeignKey(
                    name: "fk_items_brands_catalog_brand_id",
                    column: x => x.catalog_brand_id,
                    principalTable: "brands",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_items_item_parents_parent_id",
                    column: x => x.parent_id,
                    principalTable: "item_parents",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_items_item_types_catalog_type_id",
                    column: x => x.catalog_type_id,
                    principalTable: "item_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_items_catalog_brand_id",
            table: "items",
            column: "catalog_brand_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_catalog_type_id",
            table: "items",
            column: "catalog_type_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_parent_id",
            table: "items",
            column: "parent_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "items");

        migrationBuilder.DropTable(
            name: "brands");

        migrationBuilder.DropTable(
            name: "item_types");

        migrationBuilder.DropSequence(
            name: "brand_hilo");

        migrationBuilder.CreateSequence(
            name: "catalog_brand_hilo",
            incrementBy: 10);

        migrationBuilder.CreateTable(
            name: "catalog_brands",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
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
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_catalog_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "Catalog",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                catalog_brand_id = table.Column<int>(type: "integer", nullable: false),
                catalog_type_id = table.Column<int>(type: "integer", nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                barcode = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                description = table.Column<string>(type: "text", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                picture_file_name = table.Column<string>(type: "text", nullable: true),
                picture_uri = table.Column<string>(type: "text", nullable: true),
                price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
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
                table.ForeignKey(
                    name: "fk_catalog_item_parents_parent_id",
                    column: x => x.parent_id,
                    principalTable: "item_parents",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_catalog_catalog_brand_id",
            table: "Catalog",
            column: "catalog_brand_id");

        migrationBuilder.CreateIndex(
            name: "ix_catalog_catalog_type_id",
            table: "Catalog",
            column: "catalog_type_id");

        migrationBuilder.CreateIndex(
            name: "ix_catalog_parent_id",
            table: "Catalog",
            column: "parent_id");
    }
}
