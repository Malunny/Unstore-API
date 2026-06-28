using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unstore.Migrations
{
    /// <inheritdoc />
    public partial class CommercialUserNameConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComercialName",
                table: "CommercialUsers",
                newName: "CommercialName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommercialName",
                table: "CommercialUsers",
                newName: "ComercialName");
        }
    }
}
