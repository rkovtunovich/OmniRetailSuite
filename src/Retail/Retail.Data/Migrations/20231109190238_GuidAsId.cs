using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retail.Api.Migrations;

/// <inheritdoc />
public partial class GuidAsId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropSequence(
            name: "catalog_item_hilo");

        migrationBuilder.DropSequence(
            name: "receipt_item_hilo");

        migrationBuilder.RenameColumn(
            name: "number",
            table: "receipt_items",
            newName: "line_number");

        migrationBuilder.AlterColumn<Guid>(
            name: "cashier_id",
            table: "receipts",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "receipts",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer")
            .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AlterColumn<Guid>(
            name: "receipt_id",
            table: "receipt_items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_item_id",
            table: "receipt_items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "receipt_items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "catalog_items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "cashiers",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer")
            .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "line_number",
            table: "receipt_items",
            newName: "number");

        migrationBuilder.CreateSequence(
            name: "catalog_item_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "receipt_item_hilo",
            incrementBy: 10);

        migrationBuilder.AlterColumn<int>(
            name: "cashier_id",
            table: "receipts",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "receipts",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid")
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AlterColumn<int>(
            name: "receipt_id",
            table: "receipt_items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "catalog_item_id",
            table: "receipt_items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "receipt_items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "catalog_items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "cashiers",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid")
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
    }
}
