using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryOrganizer.Data.Migrations;

/// <inheritdoc />
public partial class RefactorModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Amount",
            table: "StorageItem");

        migrationBuilder.AlterColumn<double>(
            name: "BaseConversionFactor",
            table: "Unit",
            type: "float",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(double),
            oldType: "float",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "BaseConversionFactor",
            table: "Unit",
            type: "float",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "float");

        migrationBuilder.AddColumn<int>(
            name: "Amount",
            table: "StorageItem",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}
