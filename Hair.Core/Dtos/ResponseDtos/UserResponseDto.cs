﻿namespace Hair.Core.Dtos.ResponseDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? RoleName { get; set; }
        public int? CompanyId { get; set; }
        public string TokenRaw { get; set; }
        public string? CompanyName { get; set; }
    }
}
