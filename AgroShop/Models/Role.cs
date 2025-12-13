namespace AgroShop.Web.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } = null!;
        public ICollection<User>? Users { get; set; }
    }
}
