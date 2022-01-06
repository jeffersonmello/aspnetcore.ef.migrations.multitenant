using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenant.Data.Migrations.Log
{
    public partial class FixLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "request_id",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "usuarioid",
                table: "logs");

            migrationBuilder.RenameColumn(
                name: "dados",
                table: "logs",
                newName: "stack_trace");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datahora",
                table: "logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 6, 10, 54, 3, 471, DateTimeKind.Local).AddTicks(7320),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "mensagem",
                table: "logs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mensagem",
                table: "logs");

            migrationBuilder.RenameColumn(
                name: "stack_trace",
                table: "logs",
                newName: "dados");

            migrationBuilder.AlterColumn<DateTime>(
                name: "datahora",
                table: "logs",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 1, 6, 10, 54, 3, 471, DateTimeKind.Local).AddTicks(7320));

            migrationBuilder.AddColumn<string>(
                name: "request_id",
                table: "logs",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "usuarioid",
                table: "logs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
