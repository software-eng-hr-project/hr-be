using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHr.Migrations
{
    /// <inheritdoc />
    public partial class isinvited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInvited",
                table: "AbpUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInvited",
                table: "AbpUsers");
        }
    }
}
