using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixAuth.Models
{
    public class JWTTokenOptions
    {
        //谁颁发的
        public string Issuer { get; set; } = "server";

        //颁发给谁
        public string Audience { get; set; } = "client";

        //令牌密码
        public string SecurityKey { get; set; } = "a secret that needs to be at least 16 characters long";

        //对称秘钥
        public SymmetricSecurityKey Key { get; set; }

        //数字签名
        public SigningCredentials Credentials { get; set; }

        public JWTTokenOptions()
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
