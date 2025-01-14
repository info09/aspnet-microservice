using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Authorization
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly CommandCode _commandCode;
        private readonly FunctionCode _functionCode;

        public ClaimRequirementFilter(FunctionCode functionCode, CommandCode commandCode)
        {
            _commandCode = commandCode;
            _functionCode = functionCode;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionClaims = context.HttpContext.User.Claims.SingleOrDefault(i => i.Type.Equals(SystemConstants.Claims.Permissions));
            if(permissionClaims != null)
            {
                var permissions = JsonSerializer.Deserialize<List<string>>(permissionClaims.Value);
                if (!permissions.Contains(PermissionHelper.GetPermissionName(_functionCode, _commandCode)))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
