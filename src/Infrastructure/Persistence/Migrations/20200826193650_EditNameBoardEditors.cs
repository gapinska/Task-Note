using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class EditNameBoardEditors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardEditor_Boards_BoardId",
                table: "BoardEditor");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardEditor_AspNetUsers_EditorId",
                table: "BoardEditor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardEditor",
                table: "BoardEditor");

            migrationBuilder.RenameTable(
                name: "BoardEditor",
                newName: "BoardEditors");

            migrationBuilder.RenameIndex(
                name: "IX_BoardEditor_EditorId",
                table: "BoardEditors",
                newName: "IX_BoardEditors_EditorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardEditors",
                table: "BoardEditors",
                columns: new[] { "BoardId", "EditorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEditors_Boards_BoardId",
                table: "BoardEditors",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEditors_AspNetUsers_EditorId",
                table: "BoardEditors",
                column: "EditorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardEditors_Boards_BoardId",
                table: "BoardEditors");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardEditors_AspNetUsers_EditorId",
                table: "BoardEditors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardEditors",
                table: "BoardEditors");

            migrationBuilder.RenameTable(
                name: "BoardEditors",
                newName: "BoardEditor");

            migrationBuilder.RenameIndex(
                name: "IX_BoardEditors_EditorId",
                table: "BoardEditor",
                newName: "IX_BoardEditor_EditorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardEditor",
                table: "BoardEditor",
                columns: new[] { "BoardId", "EditorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEditor_Boards_BoardId",
                table: "BoardEditor",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardEditor_AspNetUsers_EditorId",
                table: "BoardEditor",
                column: "EditorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
