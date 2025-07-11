using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Dtos;

public class TokenReponse
{
    public string accessToken { get; set; }
    public string refreshToken { get; set; }
}
