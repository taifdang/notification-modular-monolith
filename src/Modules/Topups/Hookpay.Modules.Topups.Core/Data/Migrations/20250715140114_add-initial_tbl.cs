using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Topups.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class addinitial_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Topup",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "varchar(50)", nullable: true),
                    Creator = table.Column<string>(type: "varchar(20)", nullable: true),
                    TranferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topup_TransactionId",
                schema: "dbo",
                table: "Topup",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topup",
                schema: "dbo");
        }
    }
}
