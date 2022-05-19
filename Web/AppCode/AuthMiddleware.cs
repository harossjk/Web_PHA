using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    public class AuthMiddleware
    {
        readonly RequestDelegate _next;
        readonly string _authKey;

        public AuthMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _authKey = appSettings.Value.AuthKey;
        }

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrWhiteSpace(token) && token != "null")
                AuthenticateContext(context, token);

            await _next(context);
        }
          
        private void AuthenticateContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                context.Items["CorpCode"] = jwtToken.Claims.FirstOrDefault(x => x.Type == "CorpCode").Value;
                context.Items["UserId"] = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

                var menuAuth = jwtToken.Claims.FirstOrDefault(x => x.Type == "MenuAuth");
                if(menuAuth != null)
                    context.Items["MenuAuth"] = JsonConvert.DeserializeObject<Dictionary<string, int>>(menuAuth.Value);
            }
            catch(Exception)
            {
            }
        }
    }
}
