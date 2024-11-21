using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CompanyWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employeedependents",
                columns: table => new
                {
                    empdependentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dependentempno = table.Column<int>(type: "integer", nullable: false),
                    fname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sex = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    relation = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeedependents", x => x.empdependentid);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    locationid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    locationname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.locationid);
                });

            migrationBuilder.CreateTable(
                name: "departementlocations",
                columns: table => new
                {
                    deptno = table.Column<int>(type: "integer", nullable: false),
                    locationid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departementlocations", x => new { x.locationid, x.deptno });
                    table.ForeignKey(
                        name: "FK_departementlocations_locations_locationid",
                        column: x => x.locationid,
                        principalTable: "locations",
                        principalColumn: "locationid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "departements",
                columns: table => new
                {
                    deptno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    deptname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mgrempno = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departements", x => x.deptno);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    empno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    directsupervisor = table.Column<int>(type: "integer", nullable: true),
                    fname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    emplevel = table.Column<int>(type: "integer", nullable: false),
                    emptype = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phonenumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    sex = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    position = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ssn = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    salary = table.Column<int>(type: "integer", nullable: false),
                    deactivatereason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    deptno = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.empno);
                    table.ForeignKey(
                        name: "FK_employees_departements_deptno",
                        column: x => x.deptno,
                        principalTable: "departements",
                        principalColumn: "deptno");
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    projno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    projname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    projlocation = table.Column<int>(type: "integer", nullable: false),
                    deptno = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.projno);
                    table.ForeignKey(
                        name: "FK_projects_departements_deptno",
                        column: x => x.deptno,
                        principalTable: "departements",
                        principalColumn: "deptno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "worksons",
                columns: table => new
                {
                    empno = table.Column<int>(type: "integer", nullable: false),
                    projno = table.Column<int>(type: "integer", nullable: false),
                    dateworked = table.Column<DateOnly>(type: "date", nullable: false),
                    hoursworked = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worksons", x => new { x.empno, x.projno });
                    table.ForeignKey(
                        name: "FK_worksons_employees_empno",
                        column: x => x.empno,
                        principalTable: "employees",
                        principalColumn: "empno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_worksons_projects_projno",
                        column: x => x.projno,
                        principalTable: "projects",
                        principalColumn: "projno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departementlocations_deptno",
                table: "departementlocations",
                column: "deptno");

            migrationBuilder.CreateIndex(
                name: "departements_mgrempno_key",
                table: "departements",
                column: "mgrempno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_deptno",
                table: "employees",
                column: "deptno");

            migrationBuilder.CreateIndex(
                name: "IX_projects_deptno",
                table: "projects",
                column: "deptno");

            migrationBuilder.CreateIndex(
                name: "IX_worksons_projno",
                table: "worksons",
                column: "projno");

            migrationBuilder.AddForeignKey(
                name: "FK_departementlocations_departements_deptno",
                table: "departementlocations",
                column: "deptno",
                principalTable: "departements",
                principalColumn: "deptno",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_departements_employees_mgrempno",
                table: "departements",
                column: "mgrempno",
                principalTable: "employees",
                principalColumn: "empno");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_departements_deptno",
                table: "employees");

            migrationBuilder.DropTable(
                name: "departementlocations");

            migrationBuilder.DropTable(
                name: "employeedependents");

            migrationBuilder.DropTable(
                name: "worksons");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "departements");

            migrationBuilder.DropTable(
                name: "employees");
        }
    }
}
