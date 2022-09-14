using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class AddModelElementInWorkpackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelElements_IfcElements_IfcElementId",
                table: "ModelElements");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelElements_WorkPackages_WorkPackageId",
                table: "ModelElements");

            migrationBuilder.DropTable(
                name: "IfcElements");

            migrationBuilder.DropIndex(
                name: "IX_ModelElements_IfcElementId",
                table: "ModelElements");

            migrationBuilder.DropIndex(
                name: "IX_ModelElements_WorkPackageId",
                table: "ModelElements");

            migrationBuilder.DropColumn(
                name: "IfcElementId",
                table: "ModelElements");

            migrationBuilder.DropColumn(
                name: "WorkPackageId",
                table: "ModelElements");

            migrationBuilder.CreateTable(
                name: "ModelElementInWorkPackages",
                columns: table => new
                {
                    ModelElementInWorkPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModelElementId = table.Column<int>(type: "int", nullable: false),
                    WorkPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelElementInWorkPackages", x => x.ModelElementInWorkPackageId);
                    table.ForeignKey(
                        name: "FK_ModelElementInWorkPackages_ModelElements_ModelElementId",
                        column: x => x.ModelElementId,
                        principalTable: "ModelElements",
                        principalColumn: "ModelElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelElementInWorkPackages_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "WorkPackageId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ModelElementInWorkPackages_ModelElementId",
                table: "ModelElementInWorkPackages",
                column: "ModelElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelElementInWorkPackages_WorkPackageId",
                table: "ModelElementInWorkPackages",
                column: "WorkPackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelElementInWorkPackages");

            migrationBuilder.AddColumn<int>(
                name: "IfcElementId",
                table: "ModelElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkPackageId",
                table: "ModelElements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IfcElements",
                columns: table => new
                {
                    IfcElementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IfcName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IfcType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NameEe = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NameEn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PredefinedType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SystemCreated = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IfcElements", x => x.IfcElementId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ModelElements_IfcElementId",
                table: "ModelElements",
                column: "IfcElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelElements_WorkPackageId",
                table: "ModelElements",
                column: "WorkPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelElements_IfcElements_IfcElementId",
                table: "ModelElements",
                column: "IfcElementId",
                principalTable: "IfcElements",
                principalColumn: "IfcElementId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelElements_WorkPackages_WorkPackageId",
                table: "ModelElements",
                column: "WorkPackageId",
                principalTable: "WorkPackages",
                principalColumn: "WorkPackageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
