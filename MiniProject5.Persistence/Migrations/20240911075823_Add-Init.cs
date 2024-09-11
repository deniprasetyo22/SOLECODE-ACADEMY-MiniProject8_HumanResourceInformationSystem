using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MiniProject8.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "workflow",
                columns: table => new
                {
                    workflowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workflowname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow", x => x.workflowid);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflowsequences",
                columns: table => new
                {
                    stepid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workflowid = table.Column<int>(type: "integer", nullable: true),
                    steporder = table.Column<int>(type: "integer", nullable: true),
                    stepname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    requiredrole = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflowsequences", x => x.stepid);
                    table.ForeignKey(
                        name: "FK_workflowsequences_AspNetUsers_requiredrole",
                        column: x => x.requiredrole,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_workflowsequences_workflow_workflowid",
                        column: x => x.workflowid,
                        principalTable: "workflow",
                        principalColumn: "workflowid");
                });

            migrationBuilder.CreateTable(
                name: "nextsteprules",
                columns: table => new
                {
                    ruleid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    currentstepid = table.Column<int>(type: "integer", nullable: true),
                    nextstepid = table.Column<int>(type: "integer", nullable: true),
                    conditiontype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    conditionvalue = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nextsteprules", x => x.ruleid);
                    table.ForeignKey(
                        name: "FK_nextsteprules_workflowsequences_currentstepid",
                        column: x => x.currentstepid,
                        principalTable: "workflowsequences",
                        principalColumn: "stepid");
                    table.ForeignKey(
                        name: "FK_nextsteprules_workflowsequences_nextstepid",
                        column: x => x.nextstepid,
                        principalTable: "workflowsequences",
                        principalColumn: "stepid");
                });

            migrationBuilder.CreateTable(
                name: "process",
                columns: table => new
                {
                    processid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workflowid = table.Column<int>(type: "integer", nullable: true),
                    requesterid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    requesttype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    currentstepid = table.Column<int>(type: "integer", nullable: true),
                    requestdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process", x => x.processid);
                    table.ForeignKey(
                        name: "FK_process_AspNetUsers_requesterid",
                        column: x => x.requesterid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_process_workflow_workflowid",
                        column: x => x.workflowid,
                        principalTable: "workflow",
                        principalColumn: "workflowid");
                    table.ForeignKey(
                        name: "FK_process_workflowsequences_currentstepid",
                        column: x => x.currentstepid,
                        principalTable: "workflowsequences",
                        principalColumn: "stepid");
                });

            migrationBuilder.CreateTable(
                name: "workflowactions",
                columns: table => new
                {
                    actionid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    processid = table.Column<int>(type: "integer", nullable: true),
                    stepid = table.Column<int>(type: "integer", nullable: true),
                    actorid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    actiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflowactions", x => x.actionid);
                    table.ForeignKey(
                        name: "FK_workflowactions_AspNetUsers_actorid",
                        column: x => x.actorid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_workflowactions_process_processid",
                        column: x => x.processid,
                        principalTable: "process",
                        principalColumn: "processid");
                    table.ForeignKey(
                        name: "FK_workflowactions_workflowsequences_stepid",
                        column: x => x.stepid,
                        principalTable: "workflowsequences",
                        principalColumn: "stepid");
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    deptid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    deptname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    mgrempid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.deptid);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    empid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ssn = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    position = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    salary = table.Column<double>(type: "double precision", nullable: false),
                    sex = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    phoneno = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    emptype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    level = table.Column<int>(type: "integer", nullable: true),
                    deptid = table.Column<int>(type: "integer", nullable: true),
                    supervisorid = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    lastupdateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    userId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.empid);
                    table.ForeignKey(
                        name: "FK_employee_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employee_department_deptid",
                        column: x => x.deptid,
                        principalTable: "department",
                        principalColumn: "deptid");
                    table.ForeignKey(
                        name: "FK_employee_employee_supervisorid",
                        column: x => x.supervisorid,
                        principalTable: "employee",
                        principalColumn: "empid");
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    locationid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    deptid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.locationid);
                    table.ForeignKey(
                        name: "FK_location_department_deptid",
                        column: x => x.deptid,
                        principalTable: "department",
                        principalColumn: "deptid");
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    projid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    projname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    deptid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.projid);
                    table.ForeignKey(
                        name: "FK_project_department_deptid",
                        column: x => x.deptid,
                        principalTable: "department",
                        principalColumn: "deptid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dependent",
                columns: table => new
                {
                    dependentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sex = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    relationship = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    empid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dependent", x => x.dependentid);
                    table.ForeignKey(
                        name: "FK_dependent_employee_empid",
                        column: x => x.empid,
                        principalTable: "employee",
                        principalColumn: "empid");
                });

            migrationBuilder.CreateTable(
                name: "leaverequest",
                columns: table => new
                {
                    requestid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requestname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    processid = table.Column<int>(type: "integer", nullable: true),
                    empid = table.Column<int>(type: "integer", nullable: true),
                    leavetype = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaverequest", x => x.requestid);
                    table.ForeignKey(
                        name: "FK_leaverequest_employee_empid",
                        column: x => x.empid,
                        principalTable: "employee",
                        principalColumn: "empid");
                    table.ForeignKey(
                        name: "FK_leaverequest_process_processid",
                        column: x => x.processid,
                        principalTable: "process",
                        principalColumn: "processid");
                });

            migrationBuilder.CreateTable(
                name: "workson",
                columns: table => new
                {
                    empid = table.Column<int>(type: "integer", nullable: false),
                    projid = table.Column<int>(type: "integer", nullable: false),
                    dateworked = table.Column<DateOnly>(type: "date", nullable: false),
                    hoursworked = table.Column<int>(type: "integer", nullable: true),
                    userid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workson", x => new { x.empid, x.projid });
                    table.ForeignKey(
                        name: "FK_workson_AspNetUsers_userid",
                        column: x => x.userid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_workson_employee_empid",
                        column: x => x.empid,
                        principalTable: "employee",
                        principalColumn: "empid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workson_project_projid",
                        column: x => x.projid,
                        principalTable: "project",
                        principalColumn: "projid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_mgrempid",
                table: "department",
                column: "mgrempid");

            migrationBuilder.CreateIndex(
                name: "IX_dependent_empid",
                table: "dependent",
                column: "empid");

            migrationBuilder.CreateIndex(
                name: "employee_ssn_key",
                table: "employee",
                column: "ssn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_deptid",
                table: "employee",
                column: "deptid");

            migrationBuilder.CreateIndex(
                name: "IX_employee_supervisorid",
                table: "employee",
                column: "supervisorid");

            migrationBuilder.CreateIndex(
                name: "IX_employee_userId",
                table: "employee",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_leaverequest_empid",
                table: "leaverequest",
                column: "empid");

            migrationBuilder.CreateIndex(
                name: "IX_leaverequest_processid",
                table: "leaverequest",
                column: "processid");

            migrationBuilder.CreateIndex(
                name: "IX_location_deptid",
                table: "location",
                column: "deptid");

            migrationBuilder.CreateIndex(
                name: "IX_nextsteprules_currentstepid",
                table: "nextsteprules",
                column: "currentstepid");

            migrationBuilder.CreateIndex(
                name: "IX_nextsteprules_nextstepid",
                table: "nextsteprules",
                column: "nextstepid");

            migrationBuilder.CreateIndex(
                name: "IX_process_currentstepid",
                table: "process",
                column: "currentstepid");

            migrationBuilder.CreateIndex(
                name: "IX_process_requesterid",
                table: "process",
                column: "requesterid");

            migrationBuilder.CreateIndex(
                name: "IX_process_workflowid",
                table: "process",
                column: "workflowid");

            migrationBuilder.CreateIndex(
                name: "IX_project_deptid",
                table: "project",
                column: "deptid");

            migrationBuilder.CreateIndex(
                name: "IX_workflowactions_actorid",
                table: "workflowactions",
                column: "actorid");

            migrationBuilder.CreateIndex(
                name: "IX_workflowactions_processid",
                table: "workflowactions",
                column: "processid");

            migrationBuilder.CreateIndex(
                name: "IX_workflowactions_stepid",
                table: "workflowactions",
                column: "stepid");

            migrationBuilder.CreateIndex(
                name: "IX_workflowsequences_requiredrole",
                table: "workflowsequences",
                column: "requiredrole");

            migrationBuilder.CreateIndex(
                name: "IX_workflowsequences_workflowid",
                table: "workflowsequences",
                column: "workflowid");

            migrationBuilder.CreateIndex(
                name: "IX_workson_projid",
                table: "workson",
                column: "projid");

            migrationBuilder.CreateIndex(
                name: "IX_workson_userid",
                table: "workson",
                column: "userid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_department_employee_mgrempid",
                table: "department",
                column: "mgrempid",
                principalTable: "employee",
                principalColumn: "empid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_AspNetUsers_userId",
                table: "employee");

            migrationBuilder.DropForeignKey(
                name: "FK_department_employee_mgrempid",
                table: "department");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "dependent");

            migrationBuilder.DropTable(
                name: "leaverequest");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "nextsteprules");

            migrationBuilder.DropTable(
                name: "workflowactions");

            migrationBuilder.DropTable(
                name: "workson");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "process");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "workflowsequences");

            migrationBuilder.DropTable(
                name: "workflow");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "department");
        }
    }
}
