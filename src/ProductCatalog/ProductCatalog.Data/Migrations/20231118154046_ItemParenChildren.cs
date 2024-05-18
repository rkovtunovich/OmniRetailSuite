using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class ItemParenChildren : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "item_parent_id",
            table: "item_parents",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "ix_item_parents_item_parent_id",
            table: "item_parents",
            column: "item_parent_id");

        migrationBuilder.AddForeignKey(
            name: "fk_item_parents_item_parents_item_parent_id",
            table: "item_parents",
            column: "item_parent_id",
            principalTable: "item_parents",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_item_parents_item_parents_item_parent_id",
            table: "item_parents");

        migrationBuilder.DropIndex(
            name: "ix_item_parents_item_parent_id",
            table: "item_parents");

        migrationBuilder.DropColumn(
            name: "item_parent_id",
            table: "item_parents");
    }
}
