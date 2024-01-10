using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceWeb.Migrations
{
    /// <inheritdoc />
    public partial class changeDataToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderData",
                table: "OrderHeaders",
                newName: "OrderDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "OrderHeaders",
                newName: "OrderData");
        }
    }
}
