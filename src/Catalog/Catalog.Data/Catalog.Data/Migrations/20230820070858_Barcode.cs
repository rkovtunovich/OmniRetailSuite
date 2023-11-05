using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Barcode : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "barcode",
            table: "Catalog",
            type: "character varying(13)",
            maxLength: 13,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "barcode",
            table: "Catalog");
    }
}
