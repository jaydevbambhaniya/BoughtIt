using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOrder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedGlobalProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GlobalProductId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalProductId",
                table: "Products");
        }
    }
}
