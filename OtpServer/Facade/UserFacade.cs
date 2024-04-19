using OtpServer.Dto;
using OtpServer.Mapper;
using OtpServer.Repository.Model;
using OtpServer.Request;
using OtpServer.Security.Jwt;
using OtpServer.Service;

namespace OtpServer.Facade
{
    public class UserFacade : IUserFacade
    {
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFacade(IUserService userService, IUserMapper userMapper, IJwtTokenHandler jwtTokenHandler, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _userMapper = userMapper;
            _jwtTokenHandler = jwtTokenHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto> AddUserAsync(CreateUserRequest request)
        {
            var userToAdd = _userMapper.ToUser(request);
            var addedUser = await _userService.AddUserAsync(userToAdd);
            return _userMapper.ToDto(addedUser);
        }

        public async Task<string> LoginUserAsync(LoginRequest request)
        {
            User user = await _userService.GetUserByUsernameAndPasswordAsync(request.Username, request.Password);
            return _jwtTokenHandler.GenerateJwtToken(user, TokenType.MainLogin);
        }

        private HttpContext CurrentHttpContext => _httpContextAccessor.HttpContext;
    }
}
