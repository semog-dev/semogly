using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Semogly.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    middle_name = table.Column<string>(type: "varchar(30)", nullable: true),
                    last_name = table.Column<string>(type: "varchar(30)", nullable: false),
                    first_name = table.Column<string>(type: "varchar(20)", nullable: false),
                    email_address = table.Column<string>(type: "varchar(50)", nullable: false),
                    verification_code = table.Column<string>(type: "char(6)", nullable: false),
                    verification_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    verification_verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    password_hashed = table.Column<string>(type: "text", nullable: false),
                    lockout_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lockout_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
