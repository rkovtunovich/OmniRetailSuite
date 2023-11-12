using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class ItemParent : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence(
            name: "item_parent_hilo",
            incrementBy: 10);

        migrationBuilder.AddColumn<int>(
            name: "parent_id",
            table: "Catalog",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "item_parents",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false),
                name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
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

        migrationBuilder.CreateIndex(
            name: "ix_catalog_parent_id",
            table: "Catalog",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "ix_item_parents_parent_id",
            table: "item_parents",
            column: "parent_id");

        migrationBuilder.AddForeignKey(
            name: "fk_catalog_item_parents_parent_id",
            table: "Catalog",
            column: "parent_id",
            principalTable: "item_parents",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_catalog_item_parents_parent_id",
            table: "Catalog");

        migrationBuilder.DropTable(
            name: "item_parents");

        migrationBuilder.DropIndex(
            name: "ix_catalog_parent_id",
            table: "Catalog");

        migrationBuilder.DropColumn(
            name: "parent_id",
            table: "Catalog");

        migrationBuilder.DropSequence(
            name: "item_parent_hilo");
    }
}
