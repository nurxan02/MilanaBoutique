﻿// <auto-generated />
using System;
using MilanaBoutique.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MilanaBoutique.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220315113442_prodIdRelated")]
    partial class prodIdRelated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MilanaBoutique.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(65)")
                        .HasMaxLength(65);

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(160)")
                        .HasMaxLength(160);

                    b.Property<double?>("Discount")
                        .HasColumnType("float");

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)")
                        .HasMaxLength(120);

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<short?>("Rating")
                        .HasColumnType("smallint");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Subtitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)")
                        .HasMaxLength(120);

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("GenderId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MilanaBoutique.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductSizeColorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductSizeColorId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("MilanaBoutique.Models.ProductSizeColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<string>("MainImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<int>("TotalSold")
                        .HasColumnType("int");

                    b.Property<int>("TotalStock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductSizeColors");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Slider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("TextBig")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("TextSmall")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("MilanaBoutique.Models.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Category", b =>
                {
                    b.HasOne("MilanaBoutique.Models.Gender", "Gender")
                        .WithMany("Categories")
                        .HasForeignKey("GenderId");
                });

            modelBuilder.Entity("MilanaBoutique.Models.Product", b =>
                {
                    b.HasOne("MilanaBoutique.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilanaBoutique.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilanaBoutique.Models.Gender", "Gender")
                        .WithMany("Products")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilanaBoutique.Models.SubCategory", "SubCategory")
                        .WithMany("Products")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilanaBoutique.Models.ProductImage", b =>
                {
                    b.HasOne("MilanaBoutique.Models.ProductSizeColor", "ProductSizeColor")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductSizeColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilanaBoutique.Models.ProductSizeColor", b =>
                {
                    b.HasOne("MilanaBoutique.Models.Color", "Color")
                        .WithMany("ProductSizeColors")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilanaBoutique.Models.Product", "Product")
                        .WithMany("ProductSizeColors")
                        .HasForeignKey("ProductId");

                    b.HasOne("MilanaBoutique.Models.Size", "Size")
                        .WithMany("ProductSizeColors")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilanaBoutique.Models.SubCategory", b =>
                {
                    b.HasOne("MilanaBoutique.Models.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
