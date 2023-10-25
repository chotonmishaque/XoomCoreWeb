namespace XoomCore.Core.Shared
{
    public interface ISoftDelete
    {
        long? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
