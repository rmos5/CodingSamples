using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagerDemo.Data.Migrations
{
    public partial class ModifyInfotablerename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModifyInfos_Task_TaskId",
                table: "ModifyInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModifyInfos",
                table: "ModifyInfos");

            migrationBuilder.RenameTable(
                name: "ModifyInfos",
                newName: "ModifyInfo");

            migrationBuilder.RenameIndex(
                name: "IX_ModifyInfos_TaskId",
                table: "ModifyInfo",
                newName: "IX_ModifyInfo_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModifyInfo",
                table: "ModifyInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifyInfo_Task_TaskId",
                table: "ModifyInfo",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModifyInfo_Task_TaskId",
                table: "ModifyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModifyInfo",
                table: "ModifyInfo");

            migrationBuilder.RenameTable(
                name: "ModifyInfo",
                newName: "ModifyInfos");

            migrationBuilder.RenameIndex(
                name: "IX_ModifyInfo_TaskId",
                table: "ModifyInfos",
                newName: "IX_ModifyInfos_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModifyInfos",
                table: "ModifyInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifyInfos_Task_TaskId",
                table: "ModifyInfos",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
