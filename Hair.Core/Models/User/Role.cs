﻿namespace Hair.Core.Models.User
{
    public class Role : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
