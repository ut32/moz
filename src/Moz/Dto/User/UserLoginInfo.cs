namespace Moz.Dto.User
{
    public class UserLoginInfo: RegistrationInfo
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
        
        public long ExpireDateTime { get; set; }  
    }
}