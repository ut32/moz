namespace Moz.Core.Dtos.Members
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UpdateProfileRequest
    {
        public long MemberId { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string City { get; set; }
        public int Gender { get; set; }
    }
}