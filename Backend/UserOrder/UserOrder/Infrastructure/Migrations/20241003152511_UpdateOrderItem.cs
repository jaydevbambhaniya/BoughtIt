using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOrder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrdersItems");

            migrationBuilder.AlterColumn<string>(
                name: "GlobalProductId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "GlobalProductId",
                table: "OrdersItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_GlobalProductId",
                table: "Products",
                column: "GlobalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_GlobalProductId",
                table: "OrdersItems",
                column: "GlobalProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Products_GlobalProductId",
                table: "OrdersItems",
                column: "GlobalProductId",
                principalTable: "Products",
                principalColumn: "GlobalProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Products_GlobalProductId",
                table: "OrdersItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_GlobalProductId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_OrdersItems_GlobalProductId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "GlobalProductId",
                table: "OrdersItems");

            migrationBuilder.AlterColumn<string>(
                name: "GlobalProductId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_ProductId",
                table: "OrdersItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
