using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class AddedTransactionIdToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScoringStartTransactionId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ScoringStartTransactionId",
                table: "Projects",
                column: "ScoringStartTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_EthereumTransactions_ScoringStartTransactionId",
                table: "Projects",
                column: "ScoringStartTransactionId",
                principalTable: "EthereumTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_EthereumTransactions_ScoringStartTransactionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ScoringStartTransactionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ScoringStartTransactionId",
                table: "Projects");
        }
    }
}
