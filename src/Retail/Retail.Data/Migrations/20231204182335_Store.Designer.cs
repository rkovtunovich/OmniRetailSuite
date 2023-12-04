﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Retail.Data;

#nullable disable

namespace Retail.Data.Migrations
{
    [DbContext(typeof(RetailDbContext))]
    [Migration("20231204182335_Store")]
    partial class Store
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence<int>("cashier_codes");

            modelBuilder.HasSequence<int>("receipt_codes");

            modelBuilder.HasSequence<int>("store_codes");

            modelBuilder.Entity("CashierStore", b =>
                {
                    b.Property<Guid>("CashiersId")
                        .HasColumnType("uuid")
                        .HasColumnName("cashiers_id");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid")
                        .HasColumnName("store_id");

                    b.HasKey("CashiersId", "StoreId")
                        .HasName("pk_cashier_store");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_cashier_store_store_id");

                    b.ToTable("cashier_store", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.Cashier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("CodeNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("code_number")
                        .HasDefaultValueSql("nextval('\"cashier_codes\"')");

                    b.Property<string>("CodePrefix")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("code_prefix");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_cashiers");

                    b.HasIndex("CodePrefix", "CodeNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_cashiers_code_prefix_code_number");

                    b.ToTable("cashiers", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.ProductItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("CodeNumber")
                        .HasColumnType("integer")
                        .HasColumnName("code_number");

                    b.Property<string>("CodePrefix")
                        .HasColumnType("text")
                        .HasColumnName("code_prefix");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime?>("UpdatedAt")
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

                    b.Property<int>("CodeNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("code_number")
                        .HasDefaultValueSql("nextval('\"receipt_codes\"')");

                    b.Property<string>("CodePrefix")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("code_prefix");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid")
                        .HasColumnName("store_id");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total_price");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_receipts");

                    b.HasIndex("CashierId")
                        .HasDatabaseName("ix_receipts_cashier_id");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_receipts_store_id");

                    b.HasIndex("CodePrefix", "CodeNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_receipts_code_prefix_code_number");

                    b.ToTable("receipts", (string)null);
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.ReceiptItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime?>("UpdatedAt")
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

            modelBuilder.Entity("Retail.Core.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("address");

                    b.Property<int>("CodeNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("code_number")
                        .HasDefaultValueSql("nextval('\"store_codes\"')");

                    b.Property<string>("CodePrefix")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("code_prefix");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_store");

                    b.HasIndex("CodePrefix", "CodeNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_store_code_prefix_code_number");

                    b.ToTable("store", (string)null);
                });

            modelBuilder.Entity("CashierStore", b =>
                {
                    b.HasOne("Retail.Core.Entities.Cashier", null)
                        .WithMany()
                        .HasForeignKey("CashiersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cashier_store_cashiers_cashiers_id");

                    b.HasOne("Retail.Core.Entities.Store", null)
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cashier_store_store_store_id");
                });

            modelBuilder.Entity("Retail.Core.Entities.ReceiptAggregate.Receipt", b =>
                {
                    b.HasOne("Retail.Core.Entities.Cashier", "Cashier")
                        .WithMany()
                        .HasForeignKey("CashierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_receipts_cashiers_cashier_id");

                    b.HasOne("Retail.Core.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_receipts_store_store_id");

                    b.Navigation("Cashier");

                    b.Navigation("Store");
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
