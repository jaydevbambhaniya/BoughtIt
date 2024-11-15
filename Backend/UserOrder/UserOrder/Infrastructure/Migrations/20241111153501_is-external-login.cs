using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOrder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class isexternallogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExternalLogin",
                table: "Users",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExternalLogin",
                table: "Users");
        }
    }
}
