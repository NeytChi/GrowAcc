using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowAcc.Migrations
{
    /// <inheritdoc />
    public partial class UserAccountToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserAccounts",
                newName: "PasswordSalt");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "UserAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "UserAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "UserAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ConfirmedAt",
                table: "UserAccounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmToken",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmToken",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "UserAccounts");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "UserAccounts",
                newName: "Password");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmedAt",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
