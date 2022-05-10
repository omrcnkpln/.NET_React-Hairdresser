using Hair.Core.Dtos.AddDtos;
using Hair.Core.Dtos.ResponseDtos;
using Hair.Core.Dtos.UpdateDtos;
using Hair.Core.Models.User;
using Hair.Core.Results;

namespace Hair.Service.Abstract.UserService
{
    public interface IUserService
    {
        Task<DataResult<UserResponseDto>> Authenticate(string userName, string password);
        Task<DataResult<User>> Register(UserAddDto user);
        Task<DataResult<UserResponseDto>> UpdateUser(UserUpdateDto updateUserDto);
        Task<DataResult<UserResponseDto>> GetCurrentUser();
        //DataResult<UserResponseDto> ActiveUser();
        //DataResult<UserResponseDto> ActiveUserCompanyId();
        int GetUserIdFromToken(string token);
        string PasswordToHash(string password);
    }
}