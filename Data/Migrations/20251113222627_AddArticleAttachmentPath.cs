using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiarioMagna.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleAttachmentPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Articles");
        }
    }
}
