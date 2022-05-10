using Hair.Core.Models.System;

namespace Hair.Core.Models.User
{
    public class RoleMenu : BaseEntity
    {
        public Role Role { get; set; }
        public Page Page { get; set; }
    }
}
