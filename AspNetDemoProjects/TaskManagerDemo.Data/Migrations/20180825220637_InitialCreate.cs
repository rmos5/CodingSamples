using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagerDemo.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ParentTaskId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Task_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModifyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<Guid>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Modifier = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModifyInfos_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModifyInfos_TaskId",
                table: "ModifyInfos",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_Name",
                table: "Task",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_ParentTaskId",
                table: "Task",
                column: "ParentTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModifyInfos");

            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
