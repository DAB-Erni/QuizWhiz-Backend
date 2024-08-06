using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizWhizAPI.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CreatedQuizzes",
                columns: table => new
                {
                    CreatedQuizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatedQuizzes", x => x.CreatedQuizId);
                    table.ForeignKey(
                        name: "FK_CreatedQuizzes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedQuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_CreatedQuizzes_CreatedQuizId",
                        column: x => x.CreatedQuizId,
                        principalTable: "CreatedQuizzes",
                        principalColumn: "CreatedQuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TakeQuizzes",
                columns: table => new
                {
                    TakeQuizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreatedQuizId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakeQuizzes", x => x.TakeQuizId);
                    table.ForeignKey(
                        name: "FK_TakeQuizzes_CreatedQuizzes_CreatedQuizId",
                        column: x => x.CreatedQuizId,
                        principalTable: "CreatedQuizzes",
                        principalColumn: "CreatedQuizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakeQuizzes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatedQuizzes_UserId",
                table: "CreatedQuizzes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedQuizId",
                table: "Questions",
                column: "CreatedQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_TakeQuizzes_CreatedQuizId",
                table: "TakeQuizzes",
                column: "CreatedQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_TakeQuizzes_UserId",
                table: "TakeQuizzes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "TakeQuizzes");

            migrationBuilder.DropTable(
                name: "CreatedQuizzes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
