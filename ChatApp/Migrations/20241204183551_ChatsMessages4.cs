using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class ChatsMessages4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserChatModel");

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.AppUserId, x.ChatModelId });
                    table.ForeignKey(
                        name: "FK_ChatUser_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_ChatModelId",
                        column: x => x.ChatModelId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_ChatModelId",
                table: "ChatUser",
                column: "ChatModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUser");

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
    }
}
