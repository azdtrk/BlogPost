using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzadTurkSln.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Update122424 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilePhotoId",
                table: "Users",
                column: "ProfilePhotoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ProfilePhotoId",
                table: "Users",
                column: "ProfilePhotoId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ProfilePhotoId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfilePhotoId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
