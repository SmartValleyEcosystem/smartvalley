using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartValley.Data.SQL.Migrations
{
    public partial class TransactionIdMovedToScoringApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<long>(
                name: "ScoringStartTransactionId",
                table: "ScoringApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringApplications_ScoringStartTransactionId",
                table: "ScoringApplications",
                column: "ScoringStartTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringApplications_EthereumTransactions_ScoringStartTransactionId",
                table: "ScoringApplications",
                column: "ScoringStartTransactionId",
                principalTable: "EthereumTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringApplications_EthereumTransactions_ScoringStartTransactionId",
                table: "ScoringApplications");

            migrationBuilder.DropIndex(
                name: "IX_ScoringApplications_ScoringStartTransactionId",
                table: "ScoringApplications");

            migrationBuilder.DropColumn(
                name: "ScoringStartTransactionId",
                table: "ScoringApplications");

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
    }
}
