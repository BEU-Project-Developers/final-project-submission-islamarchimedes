using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class ChatsMessages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Chats_ChatModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatModelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "profilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserChatModel",
                columns: table => new
                {
                    ChatsId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatModel", x => new { x.ChatsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_AppUserChatModel_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserChatModel_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatModel_ParticipantsId",
                table: "AppUserChatModel",
                column: "ParticipantsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserChatModel");

            migrationBuilder.DropColumn(
                name: "profilePicture",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ChatModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatModelId",
                table: "AspNetUsers",
                column: "ChatModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Chats_ChatModelId",
                table: "AspNetUsers",
                column: "ChatModelId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
