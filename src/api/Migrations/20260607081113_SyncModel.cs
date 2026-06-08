using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_OrderId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Items",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Items_OrderId",
                table: "Items",
                newName: "IX_Items_OrderID");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_OrderID",
                table: "Items",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_OrderID",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "Items",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_OrderID",
                table: "Items",
                newName: "IX_Items_OrderId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Currency",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_OrderId",
                table: "Items",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
