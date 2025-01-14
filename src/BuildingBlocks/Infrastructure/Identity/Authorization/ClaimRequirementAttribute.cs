using Microsoft.AspNetCore.Mvc;
using Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Authorization
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(FunctionCode functionCode, CommandCode commandCode) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { functionCode, commandCode };
        }
    }
}
