using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using XoomCore.Core.Shared;

namespace XoomCore.Services.SessionControl;


public class SessionManager : ISessionManager
{
    private const string SessionKey = "MySession";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session;

    public SessionData Current
    {
        get
        {
            var sessionDataJson = Session.GetString(SessionKey);
            return string.IsNullOrEmpty(sessionDataJson)
                ? new SessionData()
                : JsonSerializer.Deserialize<SessionData>(sessionDataJson);
        }
        set
        {
            var serializedData = JsonSerializer.Serialize(value);
            Session.SetString(SessionKey, serializedData);
        }
    }
    public async Task<bool> CheckPermissionAsync(string _Controller, string _Action)
    {
        // Determine the appropriate permission based on the action
        return Current.ActionAuthorizedList.Any(action => action.ControllerName == _Controller && action.ActionMethod == _Action);

    }

    public async Task<Dictionary<string, bool>> CheckPermissionsAsync(string controller, string[] actions)
    {
        return actions.ToDictionary(action => action, action =>
            Current.ActionAuthorizedList.Any(a =>
                a.ControllerName == controller && a.ActionMethod == action));
    }

    public void SetActiveMenuItem(string _SubMenuKey)
    {
        if (string.IsNullOrEmpty(_SubMenuKey)) return;
        if (Current.SubMenuAuthorizedList == null) return;

        var currentSessionData = Current;

        // Set the active property for the menu item
        foreach (var subMenu in currentSessionData.SubMenuAuthorizedList)
        {
            subMenu.ActiveMenuItem = subMenu.Key.ToUpper() == _SubMenuKey.ToUpper();
        }
        // Update the session
        Current = currentSessionData;
    }
    public void SetInsertedIdentity<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // Check if the entity is an instance of AuditableEntity
        if (entity is AuditableEntity auditableEntity)
        {
            var sessionData = Current;
            var userId = sessionData.UserId;

            // Set identity properties for auditableEntity
            if (userId > 0)
            {
                auditableEntity.CreatedBy = userId;
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.UpdatedBy = userId;
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                // Set other audit-related properties if needed
            }
        }
    }
    public void SetUpdatedIdentity<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // Check if the entity is an instance of AuditableEntity
        if (entity is AuditableEntity auditableEntity)
        {
            var sessionData = Current;
            var userId = sessionData.UserId;

            // Set identity properties for auditableEntity
            if (userId > 0)
            {
                auditableEntity.UpdatedBy = userId;
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                // Set other audit-related properties if needed
            }
        }
    }
    public void SetDeletedIdentity<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        // Check if the entity is an instance of AuditableEntity
        if (entity is ISoftDelete auditableEntity)
        {
            var sessionData = Current;
            var userId = sessionData.UserId;

            // Set identity properties for auditableEntity
            if (userId > 0)
            {
                auditableEntity.DeletedBy = userId;
                auditableEntity.DeletedAt = DateTime.UtcNow;
                // Set other audit-related properties if needed
            }
        }
    }


}

