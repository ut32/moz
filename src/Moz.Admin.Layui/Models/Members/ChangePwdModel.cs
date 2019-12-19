namespace Moz.Administration.Models.Members
{
    public class ChangePwdModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}