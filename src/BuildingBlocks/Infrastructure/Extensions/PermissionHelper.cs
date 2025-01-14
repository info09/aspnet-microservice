using Shared.Common.Constants;

namespace Infrastructure.Extensions
{
    public static class PermissionHelper
    {
        public static string GetPermissionName(FunctionCode fuctionCode, CommandCode commandCode)
        {
            return string.Join(".", fuctionCode, commandCode);
        }
    }
}
