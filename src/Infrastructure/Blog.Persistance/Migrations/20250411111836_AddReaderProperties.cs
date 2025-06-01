using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddReaderProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Readers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Preferences",
                table: "Readers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveNotifications",
                table: "Readers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "ReaderSavedPosts",
                columns: table => new
                {
                    ReaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SavedPostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderSavedPosts", x => new { x.ReaderId, x.SavedPostsId });
                    table.ForeignKey(
                        name: "FK_ReaderSavedPosts_BlogPosts_SavedPostsId",
                        column: x => x.SavedPostsId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReaderSavedPosts_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReaderSavedPosts_SavedPostsId",
                table: "ReaderSavedPosts",
                column: "SavedPostsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReaderSavedPosts");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Preferences",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "ReceiveNotifications",
                table: "Readers");
        }
    }
}
