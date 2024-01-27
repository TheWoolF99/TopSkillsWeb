using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Groups_GroupId",
                table: "GroupStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Students_StudentId",
                table: "GroupStudent");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_CourseId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupStudent",
                table: "GroupStudent");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Attendance");

            migrationBuilder.RenameTable(
                name: "GroupStudent",
                newName: "GroupStudents");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudent_GroupId",
                table: "GroupStudents",
                newName: "IX_GroupStudents_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents",
                columns: new[] { "StudentId", "GroupId" });

            migrationBuilder.CreateTable(
                name: "AttendanceGroupStudent",
                columns: table => new
                {
                    attendancesAttendanceId = table.Column<int>(type: "int", nullable: false),
                    groupStudentsStudentId = table.Column<int>(type: "int", nullable: false),
                    groupStudentsGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceGroupStudent", x => new { x.attendancesAttendanceId, x.groupStudentsStudentId, x.groupStudentsGroupId });
                    table.ForeignKey(
                        name: "FK_AttendanceGroupStudent_Attendance_attendancesAttendanceId",
                        column: x => x.attendancesAttendanceId,
                        principalTable: "Attendance",
                        principalColumn: "AttendanceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceGroupStudent_GroupStudents_groupStudentsStudentId_groupStudentsGroupId",
                        columns: x => new { x.groupStudentsStudentId, x.groupStudentsGroupId },
                        principalTable: "GroupStudents",
                        principalColumns: new[] { "StudentId", "GroupId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceGroupStudent_groupStudentsStudentId_groupStudentsGroupId",
                table: "AttendanceGroupStudent",
                columns: new[] { "groupStudentsStudentId", "groupStudentsGroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Groups_GroupId",
                table: "GroupStudents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_Students_StudentId",
                table: "GroupStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Groups_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_Students_StudentId",
                table: "GroupStudents");

            migrationBuilder.DropTable(
                name: "AttendanceGroupStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents");

            migrationBuilder.RenameTable(
                name: "GroupStudents",
                newName: "GroupStudent");

            migrationBuilder.RenameIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudent",
                newName: "IX_GroupStudent_GroupId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupStudent",
                table: "GroupStudent",
                columns: new[] { "StudentId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_CourseId",
                table: "Attendance",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Courses_CourseId",
                table: "Attendance",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Students_StudentId",
                table: "Attendance",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Groups_GroupId",
                table: "GroupStudent",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Students_StudentId",
                table: "GroupStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
