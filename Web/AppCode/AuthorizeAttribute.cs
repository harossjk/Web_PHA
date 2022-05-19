using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

using Framework;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string menuId, PermMethod method) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { new Claim(menuId, Enum.GetName(method)) };
        }
    }
    
    public class PermissionFilter : IAuthorizationFilter
    {
        readonly Claim _claim;
        readonly JsonResult unAuthResult = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

        public PermissionFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Items.ContainsKey("MenuAuth"))
            {
                context.Result = unAuthResult;
                return;
            }

            var items = context.HttpContext.Items["MenuAuth"] as Dictionary<string, int>;

            if (!items.ContainsKey(_claim.Type))
            {
                context.Result = unAuthResult;
                return;
            }

            var userPerm = items.TypeKey(_claim.Type, 0);

            if (CheckPerm(userPerm, PermMethod.Admin)) // 관리자
                return;

            if (CheckPerm(userPerm, ToEnum<PermMethod>(_claim.Value))) // 해당권한
                return;

            context.Result = unAuthResult;
        }

        public T ToEnum<T>(string v)
        {
            return (T)Enum.Parse(typeof(T), v);
        }

        public static bool CheckPerm(int userMenuPerm, PermMethod method)
        {
            var bits = new BitArray(new int[] { userMenuPerm });
            return bits[ConvertEx.ConvertTo(method, (int)PermMethod.Void)];
        }
    }
}
