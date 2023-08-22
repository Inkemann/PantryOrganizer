using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryOrganizer.Data.Migrations;

/// <inheritdoc />
public partial class RefactorUnit : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Unit_UnitDimension_DimensionId",
            table: "Unit");

        migrationBuilder.DropColumn(
            name: "IsRecipeUnit",
            table: "Unit");

        migrationBuilder.DropColumn(
            name: "IsStorageUnit",
            table: "Unit");

        migrationBuilder.AlterColumn<string>(
            name: "NamePlural",
            table: "Unit",
            type: "nvarchar(80)",
            maxLength: 80,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<int>(
            name: "DimensionId",
            table: "Unit",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "AbbreviationPlural",
            table: "Unit",
            type: "nvarchar(80)",
            maxLength: 80,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Abbreviation",
            table: "Unit",
            type: "nvarchar(80)",
            maxLength: 80,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddForeignKey(
            name: "FK_Unit_UnitDimension_DimensionId",
            table: "Unit",
            column: "DimensionId",
            principalTable: "UnitDimension",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Unit_UnitDimension_DimensionId",
            table: "Unit");

        migrationBuilder.AlterColumn<string>(
            name: "NamePlural",
            table: "Unit",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(80)",
            oldMaxLength: 80);

        migrationBuilder.AlterColumn<int>(
            name: "DimensionId",
            table: "Unit",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<string>(
            name: "AbbreviationPlural",
            table: "Unit",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(80)",
            oldMaxLength: 80);

        migrationBuilder.AlterColumn<string>(
            name: "Abbreviation",
            table: "Unit",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(80)",
            oldMaxLength: 80);

        migrationBuilder.AddColumn<bool>(
            name: "IsRecipeUnit",
            table: "Unit",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsStorageUnit",
            table: "Unit",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("0f915c81-ac17-418a-95dd-0db852cc1af7"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("20a0bda9-cecf-4479-9f6f-d0ef75a9798f"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("2fb42003-5924-48c1-9684-be445a0da347"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("441a63f4-0211-41fd-a4b5-f4d10ddfd7c7"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("4a7f38ca-6da4-4256-9d27-8c761fd39ce1"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("584dfc68-3e8e-4923-9f9c-b9b9b2f78381"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("84d92e02-a45a-425c-b139-4bee471500b9"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.UpdateData(
            table: "Unit",
            keyColumn: "Id",
            keyValue: new Guid("c44a6fe7-4548-4754-944b-88e1dbf5e77a"),
            columns: new[] { "IsRecipeUnit", "IsStorageUnit" },
            values: new object[] { false, false });

        migrationBuilder.AddForeignKey(
            name: "FK_Unit_UnitDimension_DimensionId",
            table: "Unit",
            column: "DimensionId",
            principalTable: "UnitDimension",
            principalColumn: "Id");
    }
}
