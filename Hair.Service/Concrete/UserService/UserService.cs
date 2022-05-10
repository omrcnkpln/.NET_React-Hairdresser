using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Hair.Core.Dtos.AddDtos;
using Hair.Core.Dtos.ResponseDtos;
using Hair.Core.Dtos.UpdateDtos;
using Hair.Core.Models.User;
using Hair.Core.Results;
using Hair.Repository.Abstract;
using Hair.Service.Abstract.UserService;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hair.Service.Concrete.UserService
{
    public class UserService : IUserService
    {
        //private readonly string HashSecret = "u8x/A?D*G-KaPdSg";
        //private readonly string JwtSecret = "$B&E(H+MbQeThWmZq4t7w!z%C*F-J@Nc";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;


        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _config = config;
        }

        public string PasswordToHash(string password)
        {

            byte[] salt = Encoding.ASCII.GetBytes(_config["HashSecret"]);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
            return hashed;
        }

        //TODO: Last login attempt added to context;
        public async Task<DataResult<UserResponseDto>> Authenticate(string userName, string password)
        {
            var findedUser = await GetUserWithUserName(userName);

            if (findedUser != null)
            {
                if (findedUser?.Password == PasswordToHash(password))
                {
                    //Log.Information($"User found with userName: {userName} and login credential correct", userName);
                    var userReposData = await _unitOfWork.User.GetAsync(x => x.Id == findedUser.Id);
                    userReposData.LastLoginDate = DateTime.Now;

                    await _unitOfWork.User.UpdateAsync(userReposData);
                    await _unitOfWork.SaveChanges();

                    var jwtGeneratedUser = generateJwtToken(findedUser);

                    if (jwtGeneratedUser != null)
                    {
                        var userResponse = new UserResponseDto()
                        {
                            Id = jwtGeneratedUser.Id,
                            Name = jwtGeneratedUser.Name,
                            UserName = jwtGeneratedUser.UserName,
                            Email = jwtGeneratedUser.Email,
                            Address = jwtGeneratedUser.Address,
                            Phone = jwtGeneratedUser.Phone,
                            RoleName = jwtGeneratedUser.Role.Name,
                            TokenRaw = jwtGeneratedUser.TokenRaw,
                        };

                        return new DataResult<UserResponseDto>(HttpStatusCode.OK, userResponse);
                    }

                    return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "Jwt token not generated");
                }
                else
                {
                    var userReposData = await _unitOfWork.User.GetAsync(x => x.Id == findedUser.Id);
                    userReposData.AccessFailedCount += 1;

                    await _unitOfWork.User.UpdateAsync(userReposData);

                    //Log.Information("User found with userName: {userName} but password is wrong", userName);
                    return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "Wrong Password");
                }
            }

            //Log.Information("User not found with userName: {userName}", userName);
            return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "Bad Credentials");
        }

        public async Task<DataResult<UserResponseDto>> GetUserWithUserNameResponse(string userName)
        {
            try
            {
                if (!await _unitOfWork.User.AnyAsync(x => x.UserName == userName))
                {
                    return new DataResult<UserResponseDto>(HttpStatusCode.NotFound, "UserNotFound");
                }

                var user = await _unitOfWork.User.GetAsync(x => x.UserName == userName, x => x.Role);

                var userResponse = new UserResponseDto()
                {
                    Id = user.Id,
                    UserName = userName,
                    Address = user.Address,
                    Name = user.Name,
                    Phone = user.Phone,
                    RoleName = user.Role.Name,
                    TokenRaw = user.TokenRaw,
                    Email = user.Email,
                };

                return new DataResult<UserResponseDto>(HttpStatusCode.OK, userResponse);

            }
            catch (Exception e)
            {
                return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "UserName Response getirilemedi");
            }
        }

        public async Task<DataResult<User>> Register(UserAddDto createUser)
        {
            try
            {
                //Log.Information("Registering user {@user}", createUser.Name);
                createUser.Password = PasswordToHash(createUser.Password);

                if (await _unitOfWork.User.AnyAsync(u => u.UserName == createUser.UserName))
                {
                    //Log.Information("User {@user} already exists", createUser.Name);
                    return new DataResult<User>(HttpStatusCode.BadRequest, "UserName already exists");
                }

                var user = _mapper.Map<User>(createUser);

                //böyle bir company ve role bulunmama durumları ?
                user.Role = await _unitOfWork.Role.GetAsync(x => x.Name == "default");

                await _unitOfWork.User.AddAsync(user);
                await _unitOfWork.SaveChanges();

                var jwtGeneratedUser = generateJwtToken(user);

                if (jwtGeneratedUser != null)
                {
                    return new DataResult<User>(HttpStatusCode.OK, jwtGeneratedUser);
                }

                return new DataResult<User>(HttpStatusCode.BadRequest, "Something Went Wrong");
            }
            catch (Exception e)
            {
                return new DataResult<User>(HttpStatusCode.BadRequest, "Something Went Wrong While Registering");
            }

        }

        public async Task<DataResult<UserResponseDto>> UpdateUser(UserUpdateDto userUpdateDto)
        {
            //Log.Information("Updating user {@user}", userUpdateDto.Name);

            var activeUser = this.ActiveUser().Data;
            var activeUserId = activeUser.Id;
            var user = await _unitOfWork.User.GetAsync(x => x.Id == activeUserId, x => x.Role);

            try
            {
                if (!string.IsNullOrEmpty(userUpdateDto.Password))
                {
                    if (userUpdateDto.Password == userUpdateDto.ConfirmPassword)
                    {
                        var oldPasswordDto = PasswordToHash(userUpdateDto.OldPassword);
                        var passwordDto = PasswordToHash(userUpdateDto.Password);

                        if (user.Password == oldPasswordDto)
                        {
                            user.Password = passwordDto;
                        }
                        else
                        {
                            //Log.Warning("Password update error for User", user);
                            return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "Old Password Wrong");
                        }
                    }
                    else
                    {
                        //Log.Warning("Password update error for User", user);
                        return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "Passwords Are Not Matching");
                    }
                }

                user.Address = userUpdateDto.Address == null ? user.Address : userUpdateDto.Address;
                user.Phone = userUpdateDto.Phone == null ? user.Phone : userUpdateDto.Phone;
                user.Email = userUpdateDto.Email == null ? user.Email : userUpdateDto.Email;

                var userResponse = new UserResponseDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    RoleName = user.Role.Name,
                    TokenRaw = user.TokenRaw,
                };

                await _unitOfWork.User.UpdateAsync(user);
                await _unitOfWork.SaveChanges();

                //Log.Information("Update success for User", userResponse);
                return new DataResult<UserResponseDto>(HttpStatusCode.OK, userResponse);
            }
            catch (Exception e)
            {
                //Log.Error("An error occurred while updating a User", e.Message);
                return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "An Error Occurred While Updating The User");
            }
        }

        public DataResult<UserResponseDto> ActiveUser()
        {
            try
            {
                var id = _httpContextAccessor.HttpContext?.User.FindFirst(x => x.Type == "id");
                var name = _httpContextAccessor.HttpContext?.User.FindFirst(x => x.Type == "name");
                var logicalRef = _httpContextAccessor.HttpContext?.User.FindFirst(x => x.Type == "logicalRef");

                var resp = new UserResponseDto()
                {
                    Id = id == null ? 0 : Convert.ToInt32(id.Value),
                    //LogicalRef = logicalRef == null ? Guid.Empty : Guid.Parse(logicalRef.Value),
                    Name = name == null ? "" : name.Value

                    //Id = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == "id")?.Value),
                    //LogicalRef = Guid.Parse(_httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == "logicalRef").Value),
                    //Name = _httpContextAccessor.HttpContext?.User.FindFirst(x => x.Type == "name")?.Value,
                };

                return new DataResult<UserResponseDto>(HttpStatusCode.OK, resp);
            }
            catch (Exception e)
            {
                return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "ActiveUserDto getirilemedi");
            }
        }

        //public DataResult<ActiveUserDto> ActiveUserCompanyId()
        //{
        //    try
        //    {
        //        var userCompanyId = _httpContextAccessor.HttpContext?.User.Claims.ToList().FirstOrDefault(x => x.Type == "companyId").Value;

        //        return new DataResult<ActiveUserDto>(new ActiveUserDto()
        //        {
        //            CompanyId = Convert.ToInt32(userCompanyId)
        //        });

        //    }
        //    catch (Exception e)
        //    {
        //        return new DataResult<ActiveUserDto>(false, null, "ActiveUserDtoCompanyId getirilemedi", e.Message);
        //    }

        //}

        public async Task<DataResult<UserResponseDto>> GetCurrentUser()
        {
            try
            {

                var activeUser = this.ActiveUser().Data;
                var activeUserId = activeUser.Id;

                if (this.ActiveUser()?.Data?.Id > 0)
                {
                    var user = _unitOfWork.User.GetAsync(x => x.Id == activeUserId);
                    var mapUser = _mapper.Map<UserResponseDto>(user);

                    if (user != null)
                    {
                        return new DataResult<UserResponseDto>(HttpStatusCode.OK, mapUser);
                    }

                    return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "User Not Found");
                }
                else
                {
                    return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "You must login to the system.");
                }
            }
            catch (Exception e)
            {
                return new DataResult<UserResponseDto>(HttpStatusCode.BadRequest, "UserResponseDto getirilemedi");
            }
        }

        //Checks Jwt Is Valid and turns user id 
        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["JwtSecret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                //var logicRef = Guid.Parse(jwtToken.Claims.First(x => x.Type == "logicalRef").Value);

                return userId;
            }
            catch
            {
                return 0;
            }
        }

        private async Task<User> GetUserWithUserName(string userName)
        {
            try
            {
                if (!await _unitOfWork.User.AnyAsync(x => x.UserName == userName))
                {
                    return null;
                }

                var user = await _unitOfWork.User.GetAsync(x => x.UserName == userName, x => x.Role);

                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private User generateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["TokenSecret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {

                    //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //new Claim(ClaimTypes.Name, user.Name),
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.Name.ToString()),
                    new Claim("logicalRef",user.LogicalRef.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(360), // Token ne kadar sure gecerli oldugunu belirtilir.
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);

                //burada password null yap�nca user da de�i�iyo ve sonra context save olunca veritaban�na null yaz�yor
                var response = _mapper.Map<User>(user);

                response.TokenRaw = token.RawData;
                //response.Password = null;

                return response;

            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}