
using BuildingBlocks.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Identity.Models;

public class Role : IdentityRole<Guid>, IVersion
{
    public long Version { get; set; }
}
