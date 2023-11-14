using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "brands",
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
                table.PrimaryKey("pk_brands", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "item_parents",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_item_parents", x => x.id);
                table.ForeignKey(
                    name: "fk_item_parents_item_parents_parent_id",
                    column: x => x.parent_id,
                    principalTable: "item_parents",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "item_types",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_item_types", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                picture_file_name = table.Column<string>(type: "text", nullable: true),
                picture_uri = table.Column<string>(type: "text", nullable: true),
                parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                catalog_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                barcode = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                catalog_brand_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
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
            name: "ix_item_parents_parent_id",
            table: "item_parents",
            column: "parent_id");

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
            name: "item_parents");

        migrationBuilder.DropTable(
            name: "item_types");
    }
}
