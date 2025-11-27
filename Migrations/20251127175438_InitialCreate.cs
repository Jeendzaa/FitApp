using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealReports",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealCalories = table.Column<int>(type: "int", nullable: false),
                    MealProtein = table.Column<int>(type: "int", nullable: false),
                    MealCarbon = table.Column<int>(type: "int", nullable: false),
                    MealFat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealReports", x => x.MealId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserCurrentWeight = table.Column<int>(type: "int", nullable: false),
                    UserBmi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DailyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DailyReportCalories = table.Column<int>(type: "int", nullable: false),
                    DailyReportFat = table.Column<double>(type: "float", nullable: true),
                    DailyReportCarbon = table.Column<double>(type: "float", nullable: true),
                    DailyReportProtein = table.Column<double>(type: "float", nullable: true),
                    DailyReportWater = table.Column<double>(type: "float", nullable: true),
                    DailyReportWeight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealEntries",
                columns: table => new
                {
                    MealEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealEntryQuantity = table.Column<int>(type: "int", nullable: false),
                    MealEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealId = table.Column<int>(type: "int", nullable: false),
                    DailyReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealEntries", x => x.MealEntryId);
                    table.ForeignKey(
                        name: "FK_MealEntries_DailyReports_DailyReportId",
                        column: x => x.DailyReportId,
                        principalTable: "DailyReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealEntries_MealReports_MealId",
                        column: x => x.MealId,
                        principalTable: "MealReports",
                        principalColumn: "MealId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_UserId",
                table: "DailyReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_DailyReportId",
                table: "MealEntries",
                column: "DailyReportId");

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_MealId",
                table: "MealEntries",
                column: "MealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealEntries");

            migrationBuilder.DropTable(
                name: "DailyReports");

            migrationBuilder.DropTable(
                name: "MealReports");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
