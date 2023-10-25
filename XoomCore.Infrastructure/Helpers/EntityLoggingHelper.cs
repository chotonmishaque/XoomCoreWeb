using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using XoomCore.Core.Entities.Report;
using XoomCore.Core.Shared;
using XoomCore.Infrastructure.Persistence.Data;

namespace XoomCore.Infrastructure.Helpers;

public class EntityLoggingHelper
{
    private readonly ApplicationDbContext _dbContext;

    public EntityLoggingHelper(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task LogEntityChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntities = _dbContext.ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        var entityLogs = new List<EntityLog>();

        foreach (var entry in modifiedEntities)
        {
            var actionType = entry.State == EntityState.Added ? "Added" : entry.State == EntityState.Modified ? "Update" : "Delete";
            var primaryRefId = entry.State == EntityState.Added ? "New Entity" : (entry.Entity as BaseEntity)?.Id.ToString(); ; // Assuming Entity has an integer primary key named Id

            string oldValues = null;
            if (entry.State == EntityState.Modified)
                oldValues = SerializeToJson(GetOriginalValues(entry));
            else if (entry.State == EntityState.Added)
                oldValues = SerializeToJson(entry.Entity);
            else if (entry.State == EntityState.Deleted)
                oldValues = SerializeToJson(entry.Entity);

            string newValues = null;
            if (entry.State != EntityState.Deleted && entry.State != EntityState.Added)
                newValues = SerializeToJson(GetCurrentValues(entry));

            var affectedColumns = GetAffectedColumns(entry);
            var insertedBy = entry.Entity.CreatedBy;
            if (actionType != "Added")
            {
                insertedBy = entry.Entity.UpdatedBy;
            }
            var entityLog = new EntityLog
            {
                EntityName = entry.Entity.GetType().Name,
                ActionType = actionType,
                PrimaryRefId = primaryRefId,
                OldValues = oldValues,
                NewValues = newValues,
                AffectedColumn = affectedColumns,
                CreatedBy = insertedBy,
                CreatedAt = DateTime.UtcNow
            };

            entityLogs.Add(entityLog);
        }

        _dbContext.EntityLogs.AddRange(entityLogs);
        //await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private Dictionary<string, string> GetOriginalValues(EntityEntry<IAuditableEntity> entry)
    {
        var originalValues = entry.Properties
            .Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue))
            .ToDictionary(p => p.Metadata.Name, p => SerializeToJson(p.OriginalValue));
        return originalValues;
    }

    private Dictionary<string, string> GetCurrentValues(EntityEntry<IAuditableEntity> entry)
    {
        var currentValues = entry.Properties
            .Where(p => p.IsModified)
            .ToDictionary(p => p.Metadata.Name, p => SerializeToJson(p.CurrentValue));
        return currentValues;
    }

    private string GetAffectedColumns(EntityEntry<IAuditableEntity> entry)
    {
        var modifiedProperties = entry.Properties
            .Where(p => p.IsModified)
            .Select(p => p.Metadata.GetColumnName());
        return string.Join(", ", modifiedProperties);
    }

    private string SerializeToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }



}
