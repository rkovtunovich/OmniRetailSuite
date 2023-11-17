﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Retail.Data;

#nullable disable

namespace Retail.Data.Migrations
{
    [DbContext(typeof(RetailDbContext))]
    partial class RetailDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.Cashier", b =>
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
                        .HasName("pk_cashiers");

                    b.ToTable("cashiers", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.ProductItem", b =>
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
                        .HasName("pk_product_items");

                    b.ToTable("product_items", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CashierId")
                        .HasColumnType("uuid")
                        .HasColumnName("cashier_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)")
                        .HasColumnName("number");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total_price");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_receipts");

                    b.HasIndex("CashierId")
                        .HasDatabaseName("ix_receipts_cashier_id");

                    b.ToTable("receipts", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.ReceiptItem", b =>
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

                    b.Property<int>("LineNumber")
                        .HasColumnType("integer")
                        .HasColumnName("line_number");

                    b.Property<Guid>("ProductItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_item_id");

                    b.Property<double>("Quantity")
                        .HasColumnType("numeric(18,3)")
                        .HasColumnName("quantity");

                    b.Property<Guid>("ReceiptId")
                        .HasColumnType("uuid")
                        .HasColumnName("receipt_id");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total_price");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("unit_price");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_receipt_items");

                    b.HasIndex("ProductItemId")
                        .HasDatabaseName("ix_receipt_items_product_item_id");

                    b.HasIndex("ReceiptId")
                        .HasDatabaseName("ix_receipt_items_receipt_id");

                    b.ToTable("receipt_items", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.Receipt", b =>
                {
                    b.HasOne("Retail.Core.Entities.ReceiptAggregate.Cashier", "Cashier")
                        .WithMany()
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_receipts_cashiers_cashier_id");

                    b.Navigation("Cashier");
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.ReceiptItem", b =>
                {
                    b.HasOne("Retail.Core.Entities.ReceiptAggregate.ProductItem", "ProductItem")
                        .WithMany()
                        .HasForeignKey("ProductItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_receipt_items_product_items_product_item_id");

                    b.HasOne("Retail.Core.Entities.ReceiptAggregate.Receipt", "Receipt")
                        .WithMany("ReceiptItems")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_receipt_items_receipts_receipt_id");

                    b.Navigation("ProductItem");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.Receipt", b =>
                {
                    b.Navigation("ReceiptItems");
                });
#pragma warning restore 612, 618
        }
    }
}
