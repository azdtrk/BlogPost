using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class _20250302 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "IdentityUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Endpoint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRoleEndpoint",
                columns: table => new
                {
                    EndpointsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoleEndpoint", x => new { x.EndpointsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoleEndpoint_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoleEndpoint_Endpoint_EndpointsId",
                        column: x => x.EndpointsId,
                        principalTable: "Endpoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRoleEndpoint_RolesId",
                table: "ApplicationUserRoleEndpoint",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRoleEndpoint");

            migrationBuilder.DropTable(
                name: "Endpoint");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "IdentityUser");
        }
    }
}
