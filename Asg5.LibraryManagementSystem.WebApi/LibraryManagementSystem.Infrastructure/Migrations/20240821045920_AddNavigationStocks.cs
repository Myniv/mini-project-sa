using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationStocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure the 'stocks' table is created
            migrationBuilder.CreateTable(
                name: "stocks",
                columns: table => new
                {
                    stockid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookid = table.Column<int>(nullable: false),
                    locationid = table.Column<int>(nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stocks", x => x.stockid);
                });

            // Create index on 'locationid'
            migrationBuilder.CreateIndex(
                name: "IX_stocks_locationid",
                table: "stocks",
                column: "locationid");

            // Add foreign key for 'bookid'
            migrationBuilder.AddForeignKey(
                name: "FK_stocks_books_bookid",
                table: "stocks",
                column: "bookid",
                principalTable: "books",
                principalColumn: "bookid",
                onDelete: ReferentialAction.Cascade);

            // Add foreign key for 'locationid'
            migrationBuilder.AddForeignKey(
                name: "FK_stocks_locations_locationid",
                table: "stocks",
                column: "locationid",
                principalTable: "locations",
                principalColumn: "locationid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_stocks_books_bookid",
                table: "stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_stocks_locations_locationid",
                table: "stocks");

            // Remove index
            migrationBuilder.DropIndex(
                name: "IX_stocks_locationid",
                table: "stocks");

            // Drop the 'stocks' table
            migrationBuilder.DropTable(
                name: "stocks");
        }
    }
}
