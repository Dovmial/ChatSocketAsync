using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStorageLayer.Migrations
{
    /// <inheritdoc />
    public partial class addFieldsForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");
        }
    }
}
