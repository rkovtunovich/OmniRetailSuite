using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Common_fields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "basket_items");

        migrationBuilder.DropTable(
            name: "order_items");

        migrationBuilder.DropTable(
            name: "baskets");

        migrationBuilder.DropTable(
            name: "orders");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "items",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "items",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "item_types",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "item_types",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "item_parents",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "item_parents",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "brands",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "brands",
            type: "timestamp with time zone",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "created_at",
            table: "items");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "items");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "item_types");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "item_types");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "item_parents");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "item_parents");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "brands");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "brands");

        migrationBuilder.CreateTable(
            name: "baskets",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                buyer_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_baskets", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "orders",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                buyer_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                order_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ship_to_address_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                ship_to_address_country = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false),
                ship_to_address_state = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                ship_to_address_street = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
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
                basket_id = table.Column<int>(type: "integer", nullable: false),
                catalog_item_id = table.Column<int>(type: "integer", nullable: false),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                quantity = table.Column<int>(type: "integer", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
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
            name: "order_items",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                order_id = table.Column<int>(type: "integer", nullable: true),
                unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                units = table.Column<int>(type: "integer", nullable: false),
                item_ordered_catalog_item_id = table.Column<int>(type: "integer", nullable: false),
                item_ordered_picture_uri = table.Column<string>(type: "text", nullable: false),
                item_ordered_product_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
            name: "ix_order_items_order_id",
            table: "order_items",
            column: "order_id");
    }
}
