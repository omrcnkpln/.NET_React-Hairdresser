using Hair.Core.Dtos;
using Hair.Core.Dtos.AddDtos;
using Hair.Core.Dtos.ResponseDtos;
using Hair.Core.Dtos.UpdateDtos;
using Hair.Core.Models.User;
using Hair.Service.Abstract.UserService;
using Hair.Service.Concrete.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hair.Api.Controllers.UserController
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var result = await _userService.Authenticate(userLoginDto.UserName, userLoginDto.Password);

            if (result != null)
            {
                IActionResult apiResult = new ResultBuilder<UserResponseDto>(result.StatusCode, result.Data, result.Message).Go();

                return apiResult;
            }

            return BadRequest("An Error Occured On Controller");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(UserAddDto createUser)
        {
            var result = await _userService.Register(createUser);

            if (result != null)
            {
                IActionResult apiResult = new ResultBuilder<User>(result.StatusCode, result.Data, result.Message).Go();

                return apiResult;
            }

            return BadRequest("An Error Occured On Controller");
        }

        [HttpGet]
        [Authorize("ActionUserPolicy")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _userService.GetCurrentUser();

            if (result != null)
            {
                IActionResult apiResult = new ResultBuilder<UserResponseDto>(result.StatusCode, result.Data, result.Message).Go();

                return apiResult;
            }

            return BadRequest("An Error Occured On Controller");

            //return !result.Succeeded ? BadRequest(result.ErrorDefination) : Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            //Log.Information("Trying To Update A User On Controller");
            var result = await _userService.UpdateUser(userUpdateDto);

            if (result != null)
            {
                IActionResult apiResult = new ResultBuilder<UserResponseDto>(result.StatusCode, result.Data, result.Message).Go();

                return apiResult;
            }

            return BadRequest("An Error Occured On Controller");
        }
    }
}
