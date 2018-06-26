using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class EtheriumTransactionHasBeenLinkedWithRelatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                table: "EthereumTransactions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "EntityId",
                table: "EthereumTransactions",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "EthereumTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EntityType",
                table: "EthereumTransactions",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "EthereumTransactions");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "EthereumTransactions");
        }
    }
}
