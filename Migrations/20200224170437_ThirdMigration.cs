using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackBelt.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actividad",
                columns: table => new
                {
                    ActividadId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Tiempo = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    DurationSpan = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividad", x => x.ActividadId);
                    table.ForeignKey(
                        name: "FK_Actividad_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActividadRelation",
                columns: table => new
                {
                    UserActividadRelationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ActividadId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActividadRelation", x => x.UserActividadRelationId);
                    table.ForeignKey(
                        name: "FK_UserActividadRelation_Actividad_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividad",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActividadRelation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actividad_UserId",
                table: "Actividad",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActividadRelation_ActividadId",
                table: "UserActividadRelation",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActividadRelation_UserId",
                table: "UserActividadRelation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActividadRelation");

            migrationBuilder.DropTable(
                name: "Actividad");
        }
    }
}
