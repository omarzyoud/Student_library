using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCR.API.Migrations
{
    /// <inheritdoc />
    public partial class ahmad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CatId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StdUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StdName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsuInst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StdId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    StdId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_Materials_Students_StdId",
                        column: x => x.StdId,
                        principalTable: "Students",
                        principalColumn: "StdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materials_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCats",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCats", x => new { x.SubjectId, x.CatId });
                    table.ForeignKey(
                        name: "FK_SubCats_Categories_CatId",
                        column: x => x.CatId,
                        principalTable: "Categories",
                        principalColumn: "CatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCats_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    StdId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => new { x.StdId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_Bookmarks_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_Bookmarks_Students_StdId",
                        column: x => x.StdId,
                        principalTable: "Students",
                        principalColumn: "StdId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    StdId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => new { x.StdId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_Comments_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Students_StdId",
                        column: x => x.StdId,
                        principalTable: "Students",
                        principalColumn: "StdId");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    StdId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Degree = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => new { x.StdId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_Rates_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rates_Students_StdId",
                        column: x => x.StdId,
                        principalTable: "Students",
                        principalColumn: "StdId");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    StdId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => new { x.StdId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_Reports_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Students_StdId",
                        column: x => x.StdId,
                        principalTable: "Students",
                        principalColumn: "StdId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_MaterialId",
                table: "Bookmarks",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MaterialId",
                table: "Comments",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_StdId",
                table: "Materials",
                column: "StdId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SubjectId",
                table: "Materials",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_MaterialId",
                table: "Rates",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MaterialId",
                table: "Reports",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCats_CatId",
                table: "SubCats",
                column: "CatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SubCats");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
