namespace Moz.Auth.Attributes
{
    public class MemberAuthAttribute:MozAuthAttribute
    {
        public MemberAuthAttribute() : 
            base("member_authorize")
        { 
        }
    }
}