﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PantryOrganizer.Data;

#nullable disable

namespace PantryOrganizer.Data.Migrations
{
    [DbContext(typeof(PantryOrganizerContext))]
    [Migration("20230820085630_SeedUntData")]
    partial class SeedUntData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PantryOrganizer.Data.Models.Pantry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("Id");

                    b.ToTable("Pantry", (string)null);
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.StorageItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Ean")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Note")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<Guid>("PantryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(12, 4)
                        .HasColumnType("decimal(12,4)");

                    b.Property<double?>("RemainingPercentage")
                        .HasPrecision(5, 4)
                        .HasColumnType("float(5)");

                    b.Property<DateTimeOffset>("StoredDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PantryId");

                    b.HasIndex("UnitId");

                    b.ToTable("StorageItem", (string)null);
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AbbreviationPlural")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("BaseConversionFactor")
                        .HasColumnType("float");

                    b.Property<int?>("DimensionId")
                        .HasColumnType("int");

                    b.Property<bool>("IsBase")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRecipeUnit")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStorageUnit")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("NamePlural")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DimensionId")
                        .IsUnique()
                        .HasFilter("[IsBase] = 1");

                    b.ToTable("Unit", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("84d92e02-a45a-425c-b139-4bee471500b9"),
                            Abbreviation = "pc.",
                            AbbreviationPlural = "pcs.",
                            BaseConversionFactor = 1.0,
                            DimensionId = 1,
                            IsBase = true,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Piece",
                            NamePlural = "Pieces"
                        },
                        new
                        {
                            Id = new Guid("2fb42003-5924-48c1-9684-be445a0da347"),
                            Abbreviation = "dz.",
                            AbbreviationPlural = "dzs.",
                            BaseConversionFactor = 0.083333333333333329,
                            DimensionId = 1,
                            IsBase = false,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Dozen",
                            NamePlural = "Dozens"
                        },
                        new
                        {
                            Id = new Guid("0f915c81-ac17-418a-95dd-0db852cc1af7"),
                            Abbreviation = "g",
                            AbbreviationPlural = "g",
                            BaseConversionFactor = 1.0,
                            DimensionId = 2,
                            IsBase = true,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Gram",
                            NamePlural = "Grams"
                        },
                        new
                        {
                            Id = new Guid("441a63f4-0211-41fd-a4b5-f4d10ddfd7c7"),
                            Abbreviation = "kg",
                            AbbreviationPlural = "kg",
                            BaseConversionFactor = 1000.0,
                            DimensionId = 2,
                            IsBase = false,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Kilogram",
                            NamePlural = "Kilograms"
                        },
                        new
                        {
                            Id = new Guid("c44a6fe7-4548-4754-944b-88e1dbf5e77a"),
                            Abbreviation = "ml",
                            AbbreviationPlural = "ml",
                            BaseConversionFactor = 0.001,
                            DimensionId = 3,
                            IsBase = false,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Milliliter",
                            NamePlural = "Milliliters"
                        },
                        new
                        {
                            Id = new Guid("4a7f38ca-6da4-4256-9d27-8c761fd39ce1"),
                            Abbreviation = "l",
                            AbbreviationPlural = "l",
                            BaseConversionFactor = 1.0,
                            DimensionId = 3,
                            IsBase = true,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Liter",
                            NamePlural = "Liters"
                        },
                        new
                        {
                            Id = new Guid("584dfc68-3e8e-4923-9f9c-b9b9b2f78381"),
                            Abbreviation = "tbsp.",
                            AbbreviationPlural = "tbsps.",
                            BaseConversionFactor = 0.014999999999999999,
                            DimensionId = 3,
                            IsBase = false,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Tablespoon",
                            NamePlural = "Tablespoons"
                        },
                        new
                        {
                            Id = new Guid("20a0bda9-cecf-4479-9f6f-d0ef75a9798f"),
                            Abbreviation = "tsp.",
                            AbbreviationPlural = "tsps.",
                            BaseConversionFactor = 0.0050000000000000001,
                            DimensionId = 3,
                            IsBase = false,
                            IsRecipeUnit = false,
                            IsStorageUnit = false,
                            Name = "Teaspoon",
                            NamePlural = "Teaspoons"
                        });
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.UnitDimension", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.ToTable("UnitDimension", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Count"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Weight"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Volume"
                        });
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.StorageItem", b =>
                {
                    b.HasOne("PantryOrganizer.Data.Models.Pantry", "Pantry")
                        .WithMany("Items")
                        .HasForeignKey("PantryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryOrganizer.Data.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pantry");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.Unit", b =>
                {
                    b.HasOne("PantryOrganizer.Data.Models.UnitDimension", "Dimension")
                        .WithMany("Units")
                        .HasForeignKey("DimensionId");

                    b.Navigation("Dimension");
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.Pantry", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("PantryOrganizer.Data.Models.UnitDimension", b =>
                {
                    b.Navigation("Units");
                });
#pragma warning restore 612, 618
        }
    }
}