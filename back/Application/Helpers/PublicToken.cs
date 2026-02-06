using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Helpers;

public class PublicToken
{
    public static string GeneratePublicToken(){
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(16))
            .ToLower();
    }
}
