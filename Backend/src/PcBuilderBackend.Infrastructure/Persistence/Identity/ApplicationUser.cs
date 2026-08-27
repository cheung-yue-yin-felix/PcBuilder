using Microsoft.AspNetCore.Identity;

namespace PcBuilderBackend.Infrastructure.Persistence.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
