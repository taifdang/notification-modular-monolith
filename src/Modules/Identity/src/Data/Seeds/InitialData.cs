
using Identity.Identity.Models;

namespace Identity.Data.Seeds;

public static class InitialData
{
    public static List<User> Users { get; }
    static InitialData()
    {
        Users = new List<User> {
             new User()
             {
                Id = Guid.NewGuid(),
                FirstName = "Tobi",
                LastName = "H",
                UserName = "tobih",
                Email = "tobi@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
             },
            new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Nona",
                LastName = "M",
                UserName = "nonam",
                Email = "nona@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
           }
        };      
    }
}
