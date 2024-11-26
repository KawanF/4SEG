using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4SEG.Migrations
{
    /// <inheritdoc />
    public partial class segunda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiration",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordRecoveryToken",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordRecoveryTokenExpiration",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "OtpExpiration",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PasswordRecoveryToken",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PasswordRecoveryTokenExpiration",
                table: "Usuarios");
        }
    }
}
