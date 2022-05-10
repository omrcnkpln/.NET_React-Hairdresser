namespace Hair.Core.Dtos.UpdateDtos
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? OldPassword { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
