using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Common;

namespace Moz.Administration.Common
{
    [AdminAuth(Permissions = "admin.access")]
    public class AdminAuthBaseController : AdminBaseController
    {
        
    }
}