namespace SmartValley.Domain.Entities
{
    public class UserRole
    {
        public User User { get; set; }

        public long UserId { get; set; }

        public Role Role { get; set; }

        public long RoleId { get; set; }
    }
}