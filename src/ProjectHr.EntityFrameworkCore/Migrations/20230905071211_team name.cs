using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHr.Migrations
{
    /// <inheritdoc />
    public partial class teamname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "ProjectMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "ProjectMembers",
                type: "text",
                nullable: true);
        }
    }
}
