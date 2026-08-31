namespace PcBuilderBackend.Domain.Entities;

public class PcBuildUser : BaseEntity
{
    public Guid PcBuildId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsPublic { get; private set; }

    public PcBuild? PcBuild { get; private set; }

    protected PcBuildUser()
    {
    }

    public PcBuildUser(Guid pcBuildId, Guid userId, bool isPublic)
    {
        SetPcBuildId(pcBuildId);
        SetUserId(userId);
        IsPublic = isPublic;
    }

    public void Update(Guid pcBuildId, Guid userId, bool isPublic)
    {
        SetPcBuildId(pcBuildId);
        SetUserId(userId);
        IsPublic = isPublic;
    }

    private void SetPcBuildId(Guid pcBuildId)
    {
        if (pcBuildId == Guid.Empty)
            throw new ArgumentException("PC Build ID is required.", nameof(pcBuildId));

        PcBuildId = pcBuildId;
    }

    private void SetUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID is required.", nameof(userId));

        UserId = userId;
    }
}