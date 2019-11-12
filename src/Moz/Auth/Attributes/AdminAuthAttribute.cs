namespace Moz.Auth.Attributes
{
    public class AdminAuthAttribute:MozAuthAttribute
    {
        public AdminAuthAttribute() : 
            base("admin_authorize")
        { 
        }
    }
}