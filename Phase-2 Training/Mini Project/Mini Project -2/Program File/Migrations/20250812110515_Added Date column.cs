using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicket.Migrations
{
    /// <inheritdoc />
    public partial class AddedDatecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "dateofbirth",
                table: "Movies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateofbirth",
                table: "Movies");
        }
    }
}
