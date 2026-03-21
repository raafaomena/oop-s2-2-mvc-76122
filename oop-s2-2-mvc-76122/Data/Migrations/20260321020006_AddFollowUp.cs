using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oop_s2_2_mvc_76122.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "FollowUps");

            migrationBuilder.RenameColumn(
                name: "InspectorName",
                table: "Inspections",
                newName: "InspectionName");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "FollowUps",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "ActionRequired",
                table: "FollowUps",
                newName: "FollowUpDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InspectionName",
                table: "Inspections",
                newName: "InspectorName");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "FollowUps",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "FollowUpDate",
                table: "FollowUps",
                newName: "ActionRequired");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "FollowUps",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
