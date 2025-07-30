
using BuildingBlocks.Core.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.Identity.Models;
using System;
public class User : IdentityUser<Guid>, IVersion
{
    public long Version { get; set; }
}
