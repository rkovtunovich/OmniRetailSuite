using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Data.Migrations;

/// <inheritdoc />
public partial class ItemDescriptionNotRequired : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "description",
            table: "items",
            type: "character varying(4000)",
            maxLength: 4000,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "description",
            table: "items",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "character varying(4000)",
            oldMaxLength: 4000,
            oldNullable: true);
    }
}
