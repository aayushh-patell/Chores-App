using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    public partial class RemoveMonthModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChoreMonths_Months_MonthId",
                table: "ChoreMonths");

            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChoreMonths",
                table: "ChoreMonths");

            migrationBuilder.DropIndex(
                name: "IX_ChoreMonths_MonthId",
                table: "ChoreMonths");

            migrationBuilder.DropColumn(
                name: "MonthId",
                table: "ChoreMonths");

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "ChoreMonths",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChoreMonths",
                table: "ChoreMonths",
                columns: new[] { "ChoreId", "Month" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChoreMonths",
                table: "ChoreMonths");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "ChoreMonths");

            migrationBuilder.AddColumn<int>(
                name: "MonthId",
                table: "ChoreMonths",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChoreMonths",
                table: "ChoreMonths",
                columns: new[] { "ChoreId", "MonthId" });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Months",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "January" },
                    { 2, "February" },
                    { 3, "March" },
                    { 4, "April" },
                    { 5, "May" },
                    { 6, "June" },
                    { 7, "July" },
                    { 8, "August" },
                    { 9, "September" },
                    { 10, "October" },
                    { 11, "November" },
                    { 12, "December" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChoreMonths_MonthId",
                table: "ChoreMonths",
                column: "MonthId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoreMonths_Months_MonthId",
                table: "ChoreMonths",
                column: "MonthId",
                principalTable: "Months",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
