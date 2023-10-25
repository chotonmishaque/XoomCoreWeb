namespace XoomCore.Core.Shared
{
    public interface IAuditableEntity
    {
        public long? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
