using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "notes_datas",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    DateСhange = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Marked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes_datas", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_notes_datas_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteDataId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photos", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_photos_notes_datas_NoteDataId",
                        column: x => x.NoteDataId,
                        principalTable: "notes_datas",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notes_datas_Uid",
                table: "notes_datas",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notes_datas_UserId",
                table: "notes_datas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_photos_NoteDataId",
                table: "photos",
                column: "NoteDataId");

            migrationBuilder.CreateIndex(
                name: "IX_photos_Uid",
                table: "photos",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Uid",
                table: "users",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "photos");

            migrationBuilder.DropTable(
                name: "notes_datas");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
