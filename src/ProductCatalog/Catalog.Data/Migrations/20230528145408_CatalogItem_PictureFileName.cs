using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class CatalogItem_PictureFileName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "picture_file_name",
            table: "Catalog",
            type: "text",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "picture_file_name",
            table: "Catalog");
    }
}
