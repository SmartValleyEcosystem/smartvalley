using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EtheriumTransactionHasBeenUnlinkedFromAllotmentEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EthereumTransactions_AllotmentEvents_AllotmentEventId",
                table: "EthereumTransactions");

            migrationBuilder.DropIndex(
                name: "IX_EthereumTransactions_AllotmentEventId",
                table: "EthereumTransactions");

            migrationBuilder.DropColumn(
                name: "AllotmentEventId",
                table: "EthereumTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdating",
                table: "AllotmentEvents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpdating",
                table: "AllotmentEvents");

            migrationBuilder.AddColumn<long>(
                name: "AllotmentEventId",
                table: "EthereumTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EthereumTransactions_AllotmentEventId",
                table: "EthereumTransactions",
                column: "AllotmentEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EthereumTransactions_AllotmentEvents_AllotmentEventId",
                table: "EthereumTransactions",
                column: "AllotmentEventId",
                principalTable: "AllotmentEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
