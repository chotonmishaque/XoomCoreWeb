using Microsoft.EntityFrameworkCore;
using XoomCore.Core.Entities.AccessControl;
using XoomCore.Core.Enum;
using XoomCore.Infrastructure.Persistence.Data;

namespace XoomCore.Infrastructure.Helpers;

public class SeedDataService
{
    private readonly ApplicationDbContext _dbContext;

    public SeedDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedDataAsync()
    {
        await SeedMenusAsync();
        await SeedSubMenusAsync();
        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedUserRolesAsync();
        await SeedActionAuthorizationsAsync();
        await SeedRoleActionAuthorizationsAsync();
        // Add calls to seed other entities if needed
    }

    private async Task SeedMenusAsync()
    {
        if (!_dbContext.Menus.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Menus ON");

            _dbContext.Menus.AddRange(
                new Menu { Id = 1, Name = "DashBoard", DisplaySequence = 1, Icon = "bx bx-home-circle", Status = EntityStatus.IsActive },
                new Menu { Id = 2, Name = "Access Control", DisplaySequence = 9999, Icon = "bx bx-lock-open-alt", Status = EntityStatus.IsActive },
                new Menu { Id = 3, Name = "Reports", DisplaySequence = 999, Icon = "bx bx-cube-alt", Status = EntityStatus.IsActive }
            // ... other Menu ...
            );

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Menus OFF");

            await transaction.CommitAsync();
        }
    }


    private async Task SeedSubMenusAsync()
    {
        if (!_dbContext.SubMenus.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.SubMenus ON");
            var subMenusToAdd = new List<SubMenu>
            {
                new SubMenu { Id = 1, MenuId = 1, Key = "Home", Name = "Home", Url = "/index", DisplaySequence = 1, Status = EntityStatus.IsActive },
                new SubMenu { Id = 2, MenuId = 2, Key = "Menu", Name = "Menus", Url = "/menu/index", DisplaySequence = 1, Status = EntityStatus.IsActive },
                new SubMenu { Id = 3, MenuId = 2, Key = "SubMenu", Name = "Sub Menus", Url = "/subMenu/index", DisplaySequence = 2, Status = EntityStatus.IsActive },
                new SubMenu { Id = 4, MenuId = 2, Key = "ActionAuthorization", Name = "Action Authorizations", Url = "/actionAuthorization/index", DisplaySequence = 3, Status = EntityStatus.IsActive },
                new SubMenu { Id = 5, MenuId = 2, Key = "Role", Name = "Roles", Url = "/role/index", DisplaySequence = 4, Status = EntityStatus.IsActive },
                new SubMenu { Id = 6, MenuId = 2, Key = "RoleActionAuthorization", Name = "Role Action Authorizations", Url = "/roleActionAuthorization/index", DisplaySequence = 5, Status = EntityStatus.IsActive },
                new SubMenu { Id = 7, MenuId = 2, Key = "User", Name = "Users", Url = "/user/index", DisplaySequence = 6, Status = EntityStatus.IsActive },
                new SubMenu { Id = 8, MenuId = 2, Key = "UserRole", Name = "User Roles", Url = "/userRole/index", DisplaySequence = 7, Status = EntityStatus.IsActive },
                new SubMenu { Id = 9, MenuId = 3, Key = "EntityLog", Name = "Entity Logs", Url = "/entityLog/index", DisplaySequence = 1, Status = EntityStatus.IsActive }
                // ... other SubMenus ...
            };

            _dbContext.SubMenus.AddRange(subMenusToAdd);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.SubMenus OFF");
            await transaction.CommitAsync();

        }
    }

    private async Task SeedRolesAsync()
    {
        if (!_dbContext.Roles.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Roles ON");

            _dbContext.Roles.AddRange(
                new Role { Id = 1, Name = "Admin", Description = "Root Admin", Status = EntityStatus.IsActive },
                new Role { Id = 2, Name = "User", Description = "", Status = EntityStatus.IsActive }
            );
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Roles OFF");
            await transaction.CommitAsync();

        }
    }

    private async Task SeedUsersAsync()
    {
        if (!_dbContext.Users.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Users ON");
            _dbContext.Users.AddRange(
                new User { Id = 1, Username = "admin", Password = "$Xoom+Core$V1$10000$GQItTp3uhPonO5jFxIyYFDc4jaAFhkEYOpjDzGUecD/wyAkG", Email = "admin@gmail.com", FullName = "Admin", PhoneNumber = "0180000000", Status = UserStatus.IsActive },
                new User { Id = 2, Username = "User", Password = "$Xoom+Core$V1$10000$f23f/WffNFarzoXBnnVB/Tjm1qRMpMLnPMT2S5iT62W2PJdK", Email = "user@gmail.com", FullName = "User", PhoneNumber = "0180000000", Status = UserStatus.IsActive }
            );
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.Users OFF");
            await transaction.CommitAsync();

        }
    }

    private async Task SeedUserRolesAsync()
    {
        if (!_dbContext.UserRoles.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.UserRoles ON");
            _dbContext.UserRoles.AddRange(
                new UserRole { Id = 1, UserId = 1, RoleId = 1, Status = EntityStatus.IsActive },
                new UserRole { Id = 2, UserId = 1, RoleId = 2, Status = EntityStatus.IsActive },
                new UserRole { Id = 3, UserId = 2, RoleId = 2, Status = EntityStatus.IsActive }
            );
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.UserRoles OFF");
            await transaction.CommitAsync();

        }
    }
    private async Task SeedActionAuthorizationsAsync()
    {
        if (!_dbContext.ActionAuthorizations.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.ActionAuthorizations ON");
            var actionAuthorizationsToAdd = new List<ActionAuthorization>
            {
                new ActionAuthorization { Id = 1, SubMenuId = 1, Name = "Home", Controller = "Home", ActionMethod = "Index", IsPageLinked = 1, Description = "", Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 2, SubMenuId = 2, Name = "Menu", Controller = "Menu", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 3, SubMenuId = 2, Name = "View Menu", Controller = "Menu", ActionMethod = "GetMenuList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 4, SubMenuId = 2, Name = "Create Menu", Controller = "Menu", ActionMethod = "PostMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 5, SubMenuId = 2, Name = "Edit Menu", Controller = "Menu", ActionMethod = "PutMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 6, SubMenuId = 2, Name = "Delete Menu", Controller = "Menu", ActionMethod = "DeleteMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 7, SubMenuId = 3, Name = "Sub Menu", Controller = "SubMenu", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status =  EntityStatus.IsActive },
                new ActionAuthorization { Id = 8, SubMenuId = 3, Name = "View Sub Menu", Controller = "SubMenu", ActionMethod = "GetSubMenuList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 9, SubMenuId = 3, Name = "Create Sub Menu", Controller = "SubMenu", ActionMethod = "PostSubMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 10, SubMenuId = 3, Name = "Edit Sub Menu", Controller = "SubMenu", ActionMethod = "PutSubMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 11, SubMenuId = 3, Name = "Delete Sub Menu", Controller = "SubMenu", ActionMethod = "DeleteSubMenu", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 12, SubMenuId = 4, Name = "Action Authorization", Controller = "ActionAuthorization", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 13, SubMenuId = 4, Name = "View Action Authorization", Controller = "ActionAuthorization", ActionMethod = "GetActionAuthorizationList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 14, SubMenuId = 4, Name = "Create Action Authorization", Controller = "ActionAuthorization", ActionMethod = "PostActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 15, SubMenuId = 4, Name = "Edit View Action Authorization", Controller = "ActionAuthorization", ActionMethod = "PutActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 16, SubMenuId = 4, Name = "Delete View Action Authorization", Controller = "ActionAuthorization", ActionMethod = "DeleteActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 17, SubMenuId = 5, Name = "Role", Controller = "Role", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 18, SubMenuId = 5, Name = "View Role", Controller = "Role", ActionMethod = "GetRoleList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 19, SubMenuId = 5, Name = "Create Role", Controller = "Role", ActionMethod = "PostRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 20, SubMenuId = 5, Name = "Edit Role", Controller = "Role", ActionMethod = "PutRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 21, SubMenuId = 5, Name = "Delete Role", Controller = "Role", ActionMethod = "DeleteRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 22, SubMenuId = 6, Name = "Role Action Authorization", Controller = "RoleActionAuthorization", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 23, SubMenuId = 6, Name = "View Role Action Authorization", Controller = "RoleActionAuthorization", ActionMethod = "GetRoleActionAuthorizationList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 24, SubMenuId = 6, Name = "Create Role Action Authorization", Controller = "RoleActionAuthorization", ActionMethod = "PostRoleActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 25, SubMenuId = 6, Name = "Edit Role Action Authorization", Controller = "RoleActionAuthorization", ActionMethod = "PutRoleActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 26, SubMenuId = 6, Name = "Delete Role Action Authorization", Controller = "RoleActionAuthorization", ActionMethod = "DeleteRoleActionAuthorization", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 27, SubMenuId = 7, Name = "User", Controller = "User", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 28, SubMenuId = 7, Name = "View User", Controller = "User", ActionMethod = "GetUserList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 29, SubMenuId = 7, Name = "Create User", Controller = "User", ActionMethod = "PostUser", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 30, SubMenuId = 7, Name = "Edit User", Controller = "User", ActionMethod = "PutUser", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 31, SubMenuId = 7, Name = "Change User Password", Controller = "User", ActionMethod = "ChangeUserPassword", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 32, SubMenuId = 7, Name = "Delete User", Controller = "User", ActionMethod = "DeleteUser", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 33, SubMenuId = 8, Name = "User Role", Controller = "UserRole", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 34, SubMenuId = 8, Name = "View User Role", Controller = "UserRole", ActionMethod = "GetUserRoleList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 35, SubMenuId = 8, Name = "Create User Role", Controller = "UserRole", ActionMethod = "PostUserRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 36, SubMenuId = 8, Name = "Edit User Role", Controller = "UserRole", ActionMethod = "PutUserRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 37, SubMenuId = 8, Name = "Delete User Role", Controller = "UserRole", ActionMethod = "DeleteUserRole", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive},
                new ActionAuthorization { Id = 38, SubMenuId = 9, Name = "Entity Log", Controller = "EntityLog", ActionMethod = "Index", IsPageLinked = 1, Description = null, Status = EntityStatus.IsActive },
                new ActionAuthorization { Id = 39, SubMenuId = 9, Name = "View Entity Log", Controller = "EntityLog", ActionMethod = "GetEntityLogList", IsPageLinked = 0, Description = null, Status = EntityStatus.IsActive }
            };

            _dbContext.ActionAuthorizations.AddRange(actionAuthorizationsToAdd);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.ActionAuthorizations OFF");
            await transaction.CommitAsync();
        }
    }
    private async Task SeedRoleActionAuthorizationsAsync()
    {
        if (!_dbContext.RoleActionAuthorizations.Any())
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.RoleActionAuthorizations ON");

            var roleActionAuthorizationsToAdd = new List<RoleActionAuthorization>
            {
                new RoleActionAuthorization { Id = 1, RoleId = 1, ActionAuthorizationId = 1, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 2, RoleId = 1, ActionAuthorizationId = 2, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 3, RoleId = 1, ActionAuthorizationId = 3, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 4, RoleId = 1, ActionAuthorizationId = 4, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 5, RoleId = 1, ActionAuthorizationId = 5, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 6, RoleId = 1, ActionAuthorizationId = 6, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 7, RoleId = 1, ActionAuthorizationId = 7, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 8, RoleId = 1, ActionAuthorizationId = 8, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 9, RoleId = 1, ActionAuthorizationId = 9, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 10, RoleId = 1, ActionAuthorizationId = 10, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 11, RoleId = 1, ActionAuthorizationId = 11, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 12, RoleId = 1, ActionAuthorizationId = 12, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 13, RoleId = 1, ActionAuthorizationId = 13, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 14, RoleId = 1, ActionAuthorizationId = 14, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 15, RoleId = 1, ActionAuthorizationId = 15, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 16, RoleId = 1, ActionAuthorizationId = 16, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 17, RoleId = 1, ActionAuthorizationId = 17, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 18, RoleId = 1, ActionAuthorizationId = 18, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 19, RoleId = 1, ActionAuthorizationId = 19, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 20, RoleId = 1, ActionAuthorizationId = 20, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 21, RoleId = 1, ActionAuthorizationId = 21, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 22, RoleId = 1, ActionAuthorizationId = 22, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 23, RoleId = 1, ActionAuthorizationId = 23, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 24, RoleId = 1, ActionAuthorizationId = 24, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 25, RoleId = 1, ActionAuthorizationId = 25, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 26, RoleId = 1, ActionAuthorizationId = 26, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 27, RoleId = 1, ActionAuthorizationId = 27, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 28, RoleId = 1, ActionAuthorizationId = 28, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 29, RoleId = 1, ActionAuthorizationId = 29, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 30, RoleId = 1, ActionAuthorizationId = 30, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 31, RoleId = 1, ActionAuthorizationId = 31, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 32, RoleId = 1, ActionAuthorizationId = 32, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 33, RoleId = 1, ActionAuthorizationId = 33, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 34, RoleId = 1, ActionAuthorizationId = 34, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 35, RoleId = 1, ActionAuthorizationId = 35, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 36, RoleId = 1, ActionAuthorizationId = 36, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 37, RoleId = 1, ActionAuthorizationId = 37, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 38, RoleId = 1, ActionAuthorizationId = 38, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 39, RoleId = 1, ActionAuthorizationId = 39, Status = EntityStatus.IsActive },
                new RoleActionAuthorization { Id = 40, RoleId = 2, ActionAuthorizationId = 1, Status = EntityStatus.IsActive }
            };

            _dbContext.RoleActionAuthorizations.AddRange(roleActionAuthorizationsToAdd);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.RoleActionAuthorizations OFF");
            await transaction.CommitAsync();
        }
    }

    // Add methods to seed other entities if needed
}
