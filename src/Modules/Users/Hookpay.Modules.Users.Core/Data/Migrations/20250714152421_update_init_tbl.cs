using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hookpay.Modules.Users.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_init_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, defaultValue: ""),
                    user_password = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    user_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    user_email = table.Column<string>(type: "nvarchar(108)", maxLength: 108, nullable: false, defaultValue: ""),
                    user_phone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    is_block = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                schema: "dbo",
                columns: table => new
                {
                    set_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    set_user_id = table.Column<int>(type: "int", nullable: false),
                    disable_notification = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.set_id);
                    table.ForeignKey(
                        name: "FK_UserSetting_Users_set_user_id",
                        column: x => x.set_user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_user_email",
                schema: "dbo",
                table: "Users",
                column: "user_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting_set_user_id",
                schema: "dbo",
                table: "UserSetting",
                column: "set_user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSetting",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
