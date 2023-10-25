namespace XoomCore.Services.SessionControl;

public interface ISessionManager
{
    SessionData Current { get; set; }
    Task<bool> CheckPermissionAsync(string _Controller, string _Action);
    Task<Dictionary<string, bool>> CheckPermissionsAsync(string controller, string[] actions);
    void SetActiveMenuItem(string _SubMenuKey);
    void SetInsertedIdentity<TEntity>(TEntity entity) where TEntity : class;
    void SetUpdatedIdentity<TEntity>(TEntity entity) where TEntity : class;
    void SetDeletedIdentity<TEntity>(TEntity entity) where TEntity : class;

}
