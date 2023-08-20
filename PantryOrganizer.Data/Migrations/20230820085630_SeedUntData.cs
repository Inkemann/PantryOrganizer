using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PantryOrganizer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUntData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitDimension",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Count" },
                    { 2, "Weight" },
                    { 3, "Volume" }
                });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Id", "Abbreviation", "AbbreviationPlural", "BaseConversionFactor", "DimensionId", "IsBase", "IsRecipeUnit", "IsStorageUnit", "Name", "NamePlural" },
                values: new object[,]
                {
                    { new Guid("0f915c81-ac17-418a-95dd-0db852cc1af7"), "g", "g", 1.0, 2, true, false, false, "Gram", "Grams" },
                    { new Guid("20a0bda9-cecf-4479-9f6f-d0ef75a9798f"), "tsp.", "tsps.", 0.0050000000000000001, 3, false, false, false, "Teaspoon", "Teaspoons" },
                    { new Guid("2fb42003-5924-48c1-9684-be445a0da347"), "dz.", "dzs.", 0.083333333333333329, 1, false, false, false, "Dozen", "Dozens" },
                    { new Guid("441a63f4-0211-41fd-a4b5-f4d10ddfd7c7"), "kg", "kg", 1000.0, 2, false, false, false, "Kilogram", "Kilograms" },
                    { new Guid("4a7f38ca-6da4-4256-9d27-8c761fd39ce1"), "l", "l", 1.0, 3, true, false, false, "Liter", "Liters" },
                    { new Guid("584dfc68-3e8e-4923-9f9c-b9b9b2f78381"), "tbsp.", "tbsps.", 0.014999999999999999, 3, false, false, false, "Tablespoon", "Tablespoons" },
                    { new Guid("84d92e02-a45a-425c-b139-4bee471500b9"), "pc.", "pcs.", 1.0, 1, true, false, false, "Piece", "Pieces" },
                    { new Guid("c44a6fe7-4548-4754-944b-88e1dbf5e77a"), "ml", "ml", 0.001, 3, false, false, false, "Milliliter", "Milliliters" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("0f915c81-ac17-418a-95dd-0db852cc1af7"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("20a0bda9-cecf-4479-9f6f-d0ef75a9798f"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("2fb42003-5924-48c1-9684-be445a0da347"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("441a63f4-0211-41fd-a4b5-f4d10ddfd7c7"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("4a7f38ca-6da4-4256-9d27-8c761fd39ce1"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("584dfc68-3e8e-4923-9f9c-b9b9b2f78381"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("84d92e02-a45a-425c-b139-4bee471500b9"));

            migrationBuilder.DeleteData(
                table: "Unit",
                keyColumn: "Id",
                keyValue: new Guid("c44a6fe7-4548-4754-944b-88e1dbf5e77a"));

            migrationBuilder.DeleteData(
                table: "UnitDimension",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnitDimension",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnitDimension",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
