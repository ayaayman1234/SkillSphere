using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSphere.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeToSkillPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SkillPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "SkillPosts");
        }
    }
}
