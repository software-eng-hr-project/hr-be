using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHr.Migrations
{
    /// <inheritdoc />
    public partial class projectmembersiscontributing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContributing",
                table: "ProjectMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContributing",
                table: "ProjectMembers");
        }
    }
}
