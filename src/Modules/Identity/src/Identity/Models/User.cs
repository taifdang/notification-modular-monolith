
using BuildingBlocks.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Identity.Models;
using System;
public class User : IdentityUser<Guid>, IVersion
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public long Version { get; set; }
}
