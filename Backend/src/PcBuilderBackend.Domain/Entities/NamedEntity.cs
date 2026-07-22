using System.Runtime.CompilerServices;

namespace PcBuilderBackend.Domain.Entities;

public abstract class NamedEntity: BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public void Rename(string name)
    {
        SetName(name);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    internal void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"Product name is required.",  nameof(name));
        
        var trimmed = name.Trim();
        if (trimmed.Length > 200)
            throw new ArgumentException("Product name must be 200 characters or fewer.", nameof(name));
        
        Name = trimmed;
    }
}