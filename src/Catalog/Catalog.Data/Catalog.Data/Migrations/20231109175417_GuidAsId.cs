using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class GuidAsId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropSequence(
            name: "brand_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_hilo");

        migrationBuilder.DropSequence(
            name: "catalog_type_hilo");

        migrationBuilder.DropSequence(
            name: "item_parent_hilo");

        migrationBuilder.AlterColumn<Guid>(
            name: "parent_id",
            table: "items",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_type_id",
            table: "items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "catalog_brand_id",
            table: "items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "items",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "item_types",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "parent_id",
            table: "item_parents",
            type: "uuid",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "item_parents",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            table: "brands",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "integer");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateSequence(
            name: "brand_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "catalog_type_hilo",
            incrementBy: 10);

        migrationBuilder.CreateSequence(
            name: "item_parent_hilo",
            incrementBy: 10);

        migrationBuilder.AlterColumn<int>(
            name: "parent_id",
            table: "items",
            type: "integer",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "catalog_type_id",
            table: "items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "catalog_brand_id",
            table: "items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "items",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "item_types",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "parent_id",
            table: "item_parents",
            type: "integer",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uuid",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "item_parents",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AlterColumn<int>(
            name: "id",
            table: "brands",
            type: "integer",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");
    }
}
