using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;
using Framework;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;

namespace Web
{
    public interface IAuthService
    {
        UserEntity Authenticate(IDictionary param);
    }

    public class AuthService : BaseService, IAuthService
    {
        readonly string _authKey;

        public AuthService(IOptions<AppSettings> appSettings)
        {
            _authKey = appSettings.Value.AuthKey;
        }

        public UserEntity Authenticate(IDictionary param)
        {
            var user = UserService.LoginSelect(param);

            if (user == null)
                return null;

            string token = CreateToken(user);

            user.Token = token;

            return user;
        }

        private string CreateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim("CorpCode", user.CorpCode),
                new Claim("UserId", user.UserId),
                new Claim("MenuAuth", JsonConvert.SerializeObject(user.MenuAuthDic))
            });

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
