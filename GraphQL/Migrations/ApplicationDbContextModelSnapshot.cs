﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Basket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("BasketType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("basket_type");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("basket");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.BasketItem", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<Guid>("BasketId")
                        .HasColumnType("uuid")
                        .HasColumnName("basket_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("ItemId", "BasketId");

                    b.HasIndex("BasketId");

                    b.ToTable("basket_item");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Photo")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("photo");

                    b.HasKey("Id");

                    b.ToTable("ingredient");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Photo")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("photo");

                    b.HasKey("Id");

                    b.ToTable("item");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.ItemIngredient", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid")
                        .HasColumnName("ingredient_id");

                    b.Property<int?>("Quantity")
                        .IsRequired()
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.HasKey("ItemId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("item_ingredient");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Footer")
                        .HasColumnType("text")
                        .HasColumnName("footer");

                    b.Property<string>("Introduction")
                        .HasColumnType("text")
                        .HasColumnName("introduction");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Photo")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("photo");

                    b.HasKey("Id");

                    b.ToTable("menu");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("integer")
                        .HasColumnName("display_order");

                    b.Property<string>("Footer")
                        .HasColumnType("text")
                        .HasColumnName("footer");

                    b.Property<string>("Introduction")
                        .HasColumnType("text")
                        .HasColumnName("introduction");

                    b.Property<Guid?>("MenuId")
                        .IsRequired()
                        .HasColumnType("uuid")
                        .HasColumnName("menu_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Photo")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("photo");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("section");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.SectionItem", b =>
                {
                    b.Property<Guid>("SectionId")
                        .HasColumnType("uuid")
                        .HasColumnName("section_id");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("integer")
                        .HasColumnName("display_order");

                    b.HasKey("SectionId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("section_item");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.BasketItem", b =>
                {
                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Basket", "Basket")
                        .WithMany("BasketItems")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Basket");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.ItemIngredient", b =>
                {
                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Item", "Item")
                        .WithMany("ItemIngredients")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Section", b =>
                {
                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Menu", "Menu")
                        .WithMany("Sections")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.SectionItem", b =>
                {
                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Item", "Item")
                        .WithMany("SectionItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeDoTakeawayAPI.GraphQL.Model.Section", "Section")
                        .WithMany("SectionItems")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Basket", b =>
                {
                    b.Navigation("BasketItems");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Item", b =>
                {
                    b.Navigation("ItemIngredients");

                    b.Navigation("SectionItems");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Menu", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("WeDoTakeawayAPI.GraphQL.Model.Section", b =>
                {
                    b.Navigation("SectionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
