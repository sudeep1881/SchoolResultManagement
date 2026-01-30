using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAttendanceManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "result",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    examResult = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    isdeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__result__3213E83F3E1A0799", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Isdeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83F458D83D6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    isdeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subject__3213E83F8BF5D33F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ImageUpload = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Isdeleted = table.Column<bool>(type: "bit", nullable: true),
                    Registration_Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__3213E83FB061C1A0", x => x.id);
                    table.ForeignKey(
                        name: "FK__Registrat__RoleI__4CA06362",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    @class = table.Column<string>(name: "class", type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Section = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    SubjectsId = table.Column<int>(type: "int", nullable: false),
                    Marks = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    percentage = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    resultId = table.Column<int>(type: "int", nullable: false),
                    isdeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__studentD__3213E83F2F8603C3", x => x.id);
                    table.ForeignKey(
                        name: "FK__studentDe__Stude__68487DD7",
                        column: x => x.StudentId,
                        principalTable: "Registration",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__studentDe__Subje__693CA210",
                        column: x => x.SubjectsId,
                        principalTable: "Subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__studentDe__resul__6A30C649",
                        column: x => x.resultId,
                        principalTable: "result",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registration_RoleId",
                table: "Registration",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_studentDetails_resultId",
                table: "studentDetails",
                column: "resultId");

            migrationBuilder.CreateIndex(
                name: "IX_studentDetails_StudentId",
                table: "studentDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_studentDetails_SubjectsId",
                table: "studentDetails",
                column: "SubjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentDetails");

            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "result");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
