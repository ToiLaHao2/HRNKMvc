using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrnk.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("user_account_name")]
        public string UserAccountName { get; set; }
        [Column("user_password_hash")]
        public byte[] PasswordHash { get; set; }
        [Column("user_role")]
        public string UserRole { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("updated_by")]
        public long UpdatedBy { get; set; }

        public User()
        {

        }

        public User(long userId, string userAccountName, byte[] passwordHash, string userRole, DateTime createdAt, DateTime updatedAt, long updatedBy)
        {
            UserId = userId;
            UserAccountName = userAccountName;
            PasswordHash = passwordHash;
            UserRole = userRole;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
        }
    }
}
