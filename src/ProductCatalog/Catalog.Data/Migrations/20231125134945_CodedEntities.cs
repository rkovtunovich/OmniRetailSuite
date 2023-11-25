using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class CodedEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence<int>(
            name: "brand_codes");

        migrationBuilder.CreateSequence<int>(
            name: "item_codes");

        migrationBuilder.CreateSequence<int>(
            name: "item_parent_codes");

        migrationBuilder.CreateSequence<int>(
            name: "item_type_codes");

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "items",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"item_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "items",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "item_types",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"item_type_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "item_types",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "item_parents",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"item_parent_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "item_parents",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "code_number",
            table: "brands",
            type: "integer",
            nullable: false,
            defaultValueSql: "nextval('\"brand_codes\"')");

        migrationBuilder.AddColumn<string>(
            name: "code_prefix",
            table: "brands",
            type: "character varying(3)",
            maxLength: 3,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "ix_items_code_prefix_code_number",
            table: "items",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_item_types_code_prefix_code_number",
            table: "item_types",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_item_parents_code_prefix_code_number",
            table: "item_parents",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_brands_code_prefix_code_number",
            table: "brands",
            columns: new[] { "code_prefix", "code_number" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_items_code_prefix_code_number",
            table: "items");

        migrationBuilder.DropIndex(
            name: "ix_item_types_code_prefix_code_number",
            table: "item_types");

        migrationBuilder.DropIndex(
            name: "ix_item_parents_code_prefix_code_number",
            table: "item_parents");

        migrationBuilder.DropIndex(
            name: "ix_brands_code_prefix_code_number",
            table: "brands");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "items");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "items");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "item_types");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "item_types");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "item_parents");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "item_parents");

        migrationBuilder.DropColumn(
            name: "code_number",
            table: "brands");

        migrationBuilder.DropColumn(
            name: "code_prefix",
            table: "brands");

        migrationBuilder.DropSequence(
            name: "brand_codes");

        migrationBuilder.DropSequence(
            name: "item_codes");

        migrationBuilder.DropSequence(
            name: "item_parent_codes");

        migrationBuilder.DropSequence(
            name: "item_type_codes");
    }
}
