
namespace Hair.Core.Dtos.AddDtos
{
    public class UserAddDto
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        //public IFormFile? FormFile { get; set; }
        public int CompanyId { get; set; }
    }
}
