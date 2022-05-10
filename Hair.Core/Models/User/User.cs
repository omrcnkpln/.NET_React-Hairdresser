using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hair.Core.Models.User
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        
        public string? UserName { get; set; }
        
        public string? Email { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
        
        public string? Phone { get; set; }
        
        public string? Address { get; set; }

        public string? Image { get; set; }

        [JsonIgnore]
        public DateTime EmailVerified { get; set; }

        [JsonIgnore]
        public DateTime PhoneVerified { get; set; }

        [JsonIgnore]
        public bool EmailVerifiedDate { get; set; }

        [JsonIgnore]
        public bool PhoneVerifiedDate { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public bool IsLocked { get; set; }

        [JsonIgnore]
        public bool IsFirstLogin { get; set; }

        [JsonIgnore]
        public DateTime LastLoginDate { get; set; }

        [JsonIgnore]
        public DateTime LastPasswordChange { get; set; }


        [NotMapped]
        public string TokenRaw { get; set; }

        public Role Role { get; set; }

        [JsonIgnore]
        public int AccessFailedCount { get; set; }
    }
}
