using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Api.Migrations;

/// <inheritdoc />
public partial class common_fields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "receipt_number",
            table: "receipts",
            newName: "number");

        migrationBuilder.RenameColumn(
            name: "receipt_date",
            table: "receipts",
            newName: "date");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "receipts",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "receipts",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "receipt_items",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "receipt_items",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "catalog_items",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "catalog_items",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "created_at",
            table: "cashiers",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "updated_at",
            table: "cashiers",
            type: "timestamp with time zone",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "created_at",
            table: "receipts");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "receipts");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "receipt_items");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "receipt_items");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "catalog_items");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "catalog_items");

        migrationBuilder.DropColumn(
            name: "created_at",
            table: "cashiers");

        migrationBuilder.DropColumn(
            name: "updated_at",
            table: "cashiers");

        migrationBuilder.RenameColumn(
            name: "number",
            table: "receipts",
            newName: "receipt_number");

        migrationBuilder.RenameColumn(
            name: "date",
            table: "receipts",
            newName: "receipt_date");
    }
}
