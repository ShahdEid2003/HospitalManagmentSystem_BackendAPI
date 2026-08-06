using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_Managment_System.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LabResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: false),
                    ResultDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResults_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LabResults_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LabResults_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LabResultTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResultTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResultTranslations_LabResults_LabResultId",
                        column: x => x.LabResultId,
                        principalTable: "LabResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_CreatedById",
                table: "LabResults",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_MedicalRecordId",
                table: "LabResults",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_UpdatedById",
                table: "LabResults",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LabResultTranslations_LabResultId",
                table: "LabResultTranslations",
                column: "LabResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabResultTranslations");

            migrationBuilder.DropTable(
                name: "LabResults");
        }
    }
}
