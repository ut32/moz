using Moz.Auth.Attributes;

namespace Moz.Admin.Layui.Common
{
    [AdminAuth(Permissions = "admin.access")]
    public class AdminAuthBaseController : AdminBaseController
    {
        
    }
}