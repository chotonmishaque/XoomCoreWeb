using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Concretes.AccessControl;
using XoomCore.Infrastructure.Repositories.Concretes.Report;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;
using XoomCore.Infrastructure.Repositories.Contracts.Report;

namespace XoomCore.Infrastructure.UnitOfWorks;

/// <summary>
/// The UnitOfWork class implements the Unit of Work pattern, providing a centralized and cohesive approach
/// to manage database operations and transactions. It acts as a single entry point to the database context and
/// repositories, allowing developers to interact with the underlying database in a structured and cohesive manner.
/// Additionally, the class offers optional transaction handling for maintaining data consistency and atomicity
/// across multiple operations.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _currentTransaction;

    /// Initializes a new instance of the UnitOfWork class with the specified ApplicationDbContext.
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        MenuRepository = new MenuRepository(_dbContext);
        SubMenuRepository = new SubMenuRepository(_dbContext);
        ActionAuthorizationRepository = new ActionAuthorizationRepository(_dbContext);
        RoleActionAuthorizationRepository = new RoleActionAuthorizationRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        UserRoleRepository = new UserRoleRepository(_dbContext);
        RoleRepository = new RoleRepository(_dbContext);
        EntityLogRepository = new EntityLogRepository(_dbContext);
    }

    /// <summary>
    /// Gets the repository for MenuCategory entities.
    /// </summary>
    public IMenuRepository MenuRepository { get; }
    public ISubMenuRepository SubMenuRepository { get; }
    public IActionAuthorizationRepository ActionAuthorizationRepository { get; }
    public IRoleActionAuthorizationRepository RoleActionAuthorizationRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IEntityLogRepository EntityLogRepository { get; }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error("XoomCore: SaveChangesAsync()" + ex.Message);
            Log.Error("XoomCore: SaveChangesAsync()" + ex.StackTrace);
            if (ex.GetBaseException().GetType() == typeof(SqlException))
            {
                return -((SqlException)ex.InnerException).Number;
            }
            return -2;
        }
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
        {
            return;
        }

        try
        {
            await _dbContext.SaveChangesAsync();
            _currentTransaction.Commit();
        }
        catch
        {
            _currentTransaction.Rollback();
            throw;
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public void RollbackTransaction()
    {
        if (_currentTransaction == null)
        {
            return;
        }

        try
        {
            _currentTransaction.Rollback();
        }
        finally
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _dbContext.Dispose();
    }
}
