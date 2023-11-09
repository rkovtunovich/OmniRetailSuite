﻿// <auto-generated />
using System;
using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eShop.PublicApi.Infrastructure.Data.Migrations
{
    [DbContext(typeof(CatalogContext))]
    partial class CatalogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_brands");

                    b.ToTable("brands", (string)null);
                });

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Barcode")
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("barcode");

                    b.Property<Guid>("CatalogBrandId")
                        .HasColumnType("uuid")
                        .HasColumnName("catalog_brand_id");

                    b.Property<Guid>("CatalogTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("catalog_type_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<string>("PictureFileName")
                        .HasColumnType("text")
                        .HasColumnName("picture_file_name");

                    b.Property<string>("PictureUri")
                        .HasColumnType("text")
                        .HasColumnName("picture_uri");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_items");

                    b.HasIndex("CatalogBrandId")
                        .HasDatabaseName("ix_items_catalog_brand_id");

                    b.HasIndex("CatalogTypeId")
                        .HasDatabaseName("ix_items_catalog_type_id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_items_parent_id");

                    b.ToTable("items", (string)null);
                });

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.ItemParent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_item_parents");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_item_parents_parent_id");

                    b.ToTable("item_parents", (string)null);
                });

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.ItemType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_item_types");

                    b.ToTable("item_types", (string)null);
                });

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.Item", b =>
                {
                    b.HasOne("Catalog.Core.Entities.CatalogAggregate.Brand", "CatalogBrand")
                        .WithMany()
                        .HasForeignKey("CatalogBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_items_brands_catalog_brand_id");

                    b.HasOne("Catalog.Core.Entities.CatalogAggregate.ItemType", "CatalogType")
                        .WithMany()
                        .HasForeignKey("CatalogTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_items_item_types_catalog_type_id");

                    b.HasOne("Catalog.Core.Entities.CatalogAggregate.ItemParent", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_items_item_parents_parent_id");

                    b.Navigation("CatalogBrand");

                    b.Navigation("CatalogType");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Catalog.Core.Entities.CatalogAggregate.ItemParent", b =>
                {
                    b.HasOne("Catalog.Core.Entities.CatalogAggregate.ItemParent", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_item_parents_item_parents_parent_id");

                    b.Navigation("Parent");
                });
#pragma warning restore 612, 618
        }
    }
}
