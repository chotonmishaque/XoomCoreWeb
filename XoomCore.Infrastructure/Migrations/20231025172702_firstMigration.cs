using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XoomCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "_EntityLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryRefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EntityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EntityLogs_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Menus_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Roles_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubMenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubMenus_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubMenus_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionAuthorizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubMenuId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPageLinked = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionAuthorizations_SubMenus_SubMenuId",
                        column: x => x.SubMenuId,
                        principalTable: "SubMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionAuthorizations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionAuthorizations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleActionAuthorizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ActionAuthorizationId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleActionAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleActionAuthorizations_ActionAuthorizations_ActionAuthorizationId",
                        column: x => x.ActionAuthorizationId,
                        principalTable: "ActionAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleActionAuthorizations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleActionAuthorizations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleActionAuthorizations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "DisplaySequence", "Icon", "Name", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, null, null, null, 1, "bx bx-home-circle", "DashBoard", 1, null, null },
                    { 2L, null, null, null, 9999, "bx bx-lock-open-alt", "Access Control", 1, null, null },
                    { 3L, null, null, null, 999, "bx bx-cube-alt", "Reports", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Name", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, null, null, "Root Admin", "Admin", 1, null, null },
                    { 2L, null, null, "", "User", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateOfBirth", "DeletedAt", "DeletedBy", "Email", "FullName", "Password", "PhoneNumber", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[,]
                {
                    { 1L, null, null, null, null, null, "admin@gmail.com", "Admin", "$Xoom+Core$V1$10000$GQItTp3uhPonO5jFxIyYFDc4jaAFhkEYOpjDzGUecD/wyAkG", "0180000000", 1, null, null, "admin" },
                    { 2L, null, null, null, null, null, "user@gmail.com", "User", "$Xoom+Core$V1$10000$f23f/WffNFarzoXBnnVB/Tjm1qRMpMLnPMT2S5iT62W2PJdK", "0180000000", 1, null, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "SubMenus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DisplaySequence", "Key", "MenuId", "Name", "Status", "UpdatedAt", "UpdatedBy", "Url" },
                values: new object[,]
                {
                    { 1L, null, null, 1, "Home", 1L, "Home", 1, null, null, "/index" },
                    { 2L, null, null, 1, "Menu", 2L, "Menus", 1, null, null, "/menu/index" },
                    { 3L, null, null, 2, "SubMenu", 2L, "Sub Menus", 1, null, null, "/subMenu/index" },
                    { 4L, null, null, 3, "ActionAuthorization", 2L, "Action Authorizations", 1, null, null, "/actionAuthorization/index" },
                    { 5L, null, null, 4, "Role", 2L, "Roles", 1, null, null, "/role/index" },
                    { 6L, null, null, 5, "RoleActionAuthorization", 2L, "Role Action Authorizations", 1, null, null, "/roleActionAuthorization/index" },
                    { 7L, null, null, 6, "User", 2L, "Users", 1, null, null, "/user/index" },
                    { 8L, null, null, 7, "UserRole", 2L, "User Roles", 1, null, null, "/userRole/index" },
                    { 9L, null, null, 1, "EntityLog", 3L, "Entity Logs", 1, null, null, "/entityLog/index" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "RoleId", "Status", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1L, null, null, 1L, 1, null, null, 1L },
                    { 2L, null, null, 2L, 1, null, null, 1L },
                    { 3L, null, null, 2L, 1, null, null, 2L }
                });

            migrationBuilder.InsertData(
                table: "ActionAuthorizations",
                columns: new[] { "Id", "ActionMethod", "Controller", "CreatedAt", "CreatedBy", "Description", "IsPageLinked", "Name", "Status", "SubMenuId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, "Index", "Home", null, null, "", 1, "Home", 1, 1L, null, null },
                    { 2L, "Index", "Menu", null, null, null, 1, "Menu", 1, 2L, null, null },
                    { 3L, "GetMenuList", "Menu", null, null, null, 0, "View Menu", 1, 2L, null, null },
                    { 4L, "PostMenu", "Menu", null, null, null, 0, "Create Menu", 1, 2L, null, null },
                    { 5L, "PutMenu", "Menu", null, null, null, 0, "Edit Menu", 1, 2L, null, null },
                    { 6L, "DeleteMenu", "Menu", null, null, null, 0, "Delete Menu", 1, 2L, null, null },
                    { 7L, "Index", "SubMenu", null, null, null, 1, "Sub Menu", 1, 3L, null, null },
                    { 8L, "GetSubMenuList", "SubMenu", null, null, null, 0, "View Sub Menu", 1, 3L, null, null },
                    { 9L, "PostSubMenu", "SubMenu", null, null, null, 0, "Create Sub Menu", 1, 3L, null, null },
                    { 10L, "PutSubMenu", "SubMenu", null, null, null, 0, "Edit Sub Menu", 1, 3L, null, null },
                    { 11L, "DeleteSubMenu", "SubMenu", null, null, null, 0, "Delete Sub Menu", 1, 3L, null, null },
                    { 12L, "Index", "ActionAuthorization", null, null, null, 1, "Action Authorization", 1, 4L, null, null },
                    { 13L, "GetActionAuthorizationList", "ActionAuthorization", null, null, null, 0, "View Action Authorization", 1, 4L, null, null },
                    { 14L, "PostActionAuthorization", "ActionAuthorization", null, null, null, 0, "Create Action Authorization", 1, 4L, null, null },
                    { 15L, "PutActionAuthorization", "ActionAuthorization", null, null, null, 0, "Edit View Action Authorization", 1, 4L, null, null },
                    { 16L, "DeleteActionAuthorization", "ActionAuthorization", null, null, null, 0, "Delete View Action Authorization", 1, 4L, null, null },
                    { 17L, "Index", "Role", null, null, null, 1, "Role", 1, 5L, null, null },
                    { 18L, "GetRoleList", "Role", null, null, null, 0, "View Role", 1, 5L, null, null },
                    { 19L, "PostRole", "Role", null, null, null, 0, "Create Role", 1, 5L, null, null },
                    { 20L, "PutRole", "Role", null, null, null, 0, "Edit Role", 1, 5L, null, null },
                    { 21L, "DeleteRole", "Role", null, null, null, 0, "Delete Role", 1, 5L, null, null },
                    { 22L, "Index", "RoleActionAuthorization", null, null, null, 1, "Role Action Authorization", 1, 6L, null, null },
                    { 23L, "GetRoleActionAuthorizationList", "RoleActionAuthorization", null, null, null, 0, "View Role Action Authorization", 1, 6L, null, null },
                    { 24L, "PostRoleActionAuthorization", "RoleActionAuthorization", null, null, null, 0, "Create Role Action Authorization", 1, 6L, null, null },
                    { 25L, "PutRoleActionAuthorization", "RoleActionAuthorization", null, null, null, 0, "Edit Role Action Authorization", 1, 6L, null, null },
                    { 26L, "DeleteRoleActionAuthorization", "RoleActionAuthorization", null, null, null, 0, "Delete Role Action Authorization", 1, 6L, null, null },
                    { 27L, "Index", "User", null, null, null, 1, "User", 1, 7L, null, null },
                    { 28L, "GetUserList", "User", null, null, null, 0, "View User", 1, 7L, null, null },
                    { 29L, "PostUser", "User", null, null, null, 0, "Create User", 1, 7L, null, null },
                    { 30L, "PutUser", "User", null, null, null, 0, "Edit User", 1, 7L, null, null },
                    { 31L, "ChangeUserPassword", "User", null, null, null, 0, "Change User Password", 1, 7L, null, null },
                    { 32L, "DeleteUser", "User", null, null, null, 0, "Delete User", 1, 7L, null, null },
                    { 33L, "Index", "UserRole", null, null, null, 1, "User Role", 1, 8L, null, null },
                    { 34L, "GetUserRoleList", "UserRole", null, null, null, 0, "View User Role", 1, 8L, null, null },
                    { 35L, "PostUserRole", "UserRole", null, null, null, 0, "Create User Role", 1, 8L, null, null },
                    { 36L, "PutUserRole", "UserRole", null, null, null, 0, "Edit User Role", 1, 8L, null, null },
                    { 37L, "DeleteUserRole", "UserRole", null, null, null, 0, "Delete User Role", 1, 8L, null, null },
                    { 38L, "Index", "EntityLog", null, null, null, 1, "Entity Log", 1, 9L, null, null },
                    { 39L, "GetEntityLogList", "EntityLog", null, null, null, 0, "View Entity Log", 1, 9L, null, null }
                });

            migrationBuilder.InsertData(
                table: "RoleActionAuthorizations",
                columns: new[] { "Id", "ActionAuthorizationId", "CreatedAt", "CreatedBy", "RoleId", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, 1L, null, null, 1L, 1, null, null },
                    { 2L, 2L, null, null, 1L, 1, null, null },
                    { 3L, 3L, null, null, 1L, 1, null, null },
                    { 4L, 4L, null, null, 1L, 1, null, null },
                    { 5L, 5L, null, null, 1L, 1, null, null },
                    { 6L, 6L, null, null, 1L, 1, null, null },
                    { 7L, 7L, null, null, 1L, 1, null, null },
                    { 8L, 8L, null, null, 1L, 1, null, null },
                    { 9L, 9L, null, null, 1L, 1, null, null },
                    { 10L, 10L, null, null, 1L, 1, null, null },
                    { 11L, 11L, null, null, 1L, 1, null, null },
                    { 12L, 12L, null, null, 1L, 1, null, null },
                    { 13L, 13L, null, null, 1L, 1, null, null },
                    { 14L, 14L, null, null, 1L, 1, null, null },
                    { 15L, 15L, null, null, 1L, 1, null, null },
                    { 16L, 16L, null, null, 1L, 1, null, null },
                    { 17L, 17L, null, null, 1L, 1, null, null },
                    { 18L, 18L, null, null, 1L, 1, null, null },
                    { 19L, 19L, null, null, 1L, 1, null, null },
                    { 20L, 20L, null, null, 1L, 1, null, null },
                    { 21L, 21L, null, null, 1L, 1, null, null },
                    { 22L, 22L, null, null, 1L, 1, null, null },
                    { 23L, 23L, null, null, 1L, 1, null, null },
                    { 24L, 24L, null, null, 1L, 1, null, null },
                    { 25L, 25L, null, null, 1L, 1, null, null },
                    { 26L, 26L, null, null, 1L, 1, null, null },
                    { 27L, 27L, null, null, 1L, 1, null, null },
                    { 28L, 28L, null, null, 1L, 1, null, null },
                    { 29L, 29L, null, null, 1L, 1, null, null },
                    { 30L, 30L, null, null, 1L, 1, null, null },
                    { 31L, 31L, null, null, 1L, 1, null, null },
                    { 32L, 32L, null, null, 1L, 1, null, null },
                    { 33L, 33L, null, null, 1L, 1, null, null },
                    { 34L, 34L, null, null, 1L, 1, null, null },
                    { 35L, 35L, null, null, 1L, 1, null, null },
                    { 36L, 36L, null, null, 1L, 1, null, null },
                    { 37L, 37L, null, null, 1L, 1, null, null },
                    { 38L, 38L, null, null, 1L, 1, null, null },
                    { 39L, 39L, null, null, 1L, 1, null, null },
                    { 40L, 1L, null, null, 2L, 1, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX__EntityLogs_CreatedBy",
                table: "_EntityLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAuthorizations_CreatedBy",
                table: "ActionAuthorizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActionAuthorizations_SubMenuId_ActionMethod",
                table: "ActionAuthorizations",
                columns: new[] { "SubMenuId", "ActionMethod" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionAuthorizations_SubMenuId_Name",
                table: "ActionAuthorizations",
                columns: new[] { "SubMenuId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionAuthorizations_UpdatedBy",
                table: "ActionAuthorizations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreatedBy",
                table: "Menus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Name",
                table: "Menus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UpdatedBy",
                table: "Menus",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActionAuthorizations_ActionAuthorizationId",
                table: "RoleActionAuthorizations",
                column: "ActionAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActionAuthorizations_CreatedBy",
                table: "RoleActionAuthorizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleActionAuthorizations_RoleId_ActionAuthorizationId",
                table: "RoleActionAuthorizations",
                columns: new[] { "RoleId", "ActionAuthorizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleActionAuthorizations_UpdatedBy",
                table: "RoleActionAuthorizations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenus_CreatedBy",
                table: "SubMenus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubMenus_MenuId_Name",
                table: "SubMenus",
                columns: new[] { "MenuId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubMenus_UpdatedBy",
                table: "SubMenus",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedBy",
                table: "UserRoles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UpdatedBy",
                table: "UserRoles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedBy",
                table: "Users",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_EntityLogs");

            migrationBuilder.DropTable(
                name: "RoleActionAuthorizations");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ActionAuthorizations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SubMenus");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
