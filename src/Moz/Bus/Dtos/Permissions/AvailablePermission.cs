namespace Moz.Bus.Dtos.Permissions
{
    public class AvailablePermission
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long RoleId { get; set; }
        
        public string RoleCode { get; set; }
    }
}