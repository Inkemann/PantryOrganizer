using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryOrganizer.Data.Migrations;

/// <inheritdoc />
public partial class AddPantryModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Pantry",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Pantry", x => x.Id));

        migrationBuilder.CreateTable(
            name: "UnitDimension",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_UnitDimension", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Unit",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsBase = table.Column<bool>(type: "bit", nullable: false),
                BaseConversionFactor = table.Column<double>(type: "float", nullable: true),
                Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                AbbreviationPlural = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NamePlural = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsStorageUnit = table.Column<bool>(type: "bit", nullable: false),
                IsRecipeUnit = table.Column<bool>(type: "bit", nullable: false),
                DimensionId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Unit", x => x.Id);
                table.ForeignKey(
                    name: "FK_Unit_UnitDimension_DimensionId",
                    column: x => x.DimensionId,
                    principalTable: "UnitDimension",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "StorageItem",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                Amount = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<decimal>(type: "decimal(12,4)", precision: 12, scale: 4, nullable: false),
                RemainingPercentage = table.Column<double>(type: "float(5)", precision: 5, scale: 4, nullable: true),
                StoredDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Ean = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PantryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StorageItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_StorageItem_Pantry_PantryId",
                    column: x => x.PantryId,
                    principalTable: "Pantry",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StorageItem_Unit_UnitId",
                    column: x => x.UnitId,
                    principalTable: "Unit",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_StorageItem_PantryId",
            table: "StorageItem",
            column: "PantryId");

        migrationBuilder.CreateIndex(
            name: "IX_StorageItem_UnitId",
            table: "StorageItem",
            column: "UnitId");

        migrationBuilder.CreateIndex(
            name: "IX_Unit_DimensionId",
            table: "Unit",
            column: "DimensionId",
            unique: true,
            filter: "[IsBase] = 1");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "StorageItem");

        migrationBuilder.DropTable(
            name: "Pantry");

        migrationBuilder.DropTable(
            name: "Unit");

        migrationBuilder.DropTable(
            name: "UnitDimension");
    }
}
