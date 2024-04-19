using OtpServer.Dto;
using OtpServer.Mapper;
using OtpServer.Mapper.Hash;
using OtpServer.Otp;
using OtpServer.Repository.Model;
using OtpServer.Request;
using OtpServer.Security.Jwt;
using OtpServer.Service;

namespace OtpServer.Facade
{
    public class OtpFacade : IOtpFacade
    {
        private readonly IOtpItemService _otpItemService;
        private readonly IUserService _userService;
        private readonly IOtpItemMapper _otpItemMapper;
        private readonly IOtpProvider _otpProvider;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher _passwordHasher;

        public OtpFacade(IOtpItemService otpItemService, IHttpContextAccessor httpContextAccessor, IJwtTokenHandler jwtTokenHandler, IUserService userService, IOtpProvider otpProvider, IOtpItemMapper otpItemMapper, IPasswordHasher passwordHasher)
        {
            _otpItemService = otpItemService;
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenHandler = jwtTokenHandler;
            _userService = userService;
            _otpProvider = otpProvider;
            _otpItemMapper = otpItemMapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<OtpItemDto> GenerateOtpAsync()
        {
            var foundUser = await GetCurrentUser();

            string generatedOtp = _otpProvider.GenerateTotp(out DateTime expirationDate);
            string hashedOtp = _passwordHasher.HashPassword(generatedOtp);

            OtpItem otpItem = _otpItemMapper.MapToPendingOtpItem(foundUser, hashedOtp, expirationDate);
            OtpItem savedOtpItem = await _otpItemService.AddOtpItemAsync(otpItem);
            savedOtpItem.Otp = generatedOtp;
            return _otpItemMapper.MapToDto(savedOtpItem);
        }

        public async Task<string> LoginUserWithOtpAsync(LoginWithOtpRequest request)
        {
            var user = await GetCurrentUser();
            var otpItem = await _otpItemService.GetOtpItemByUserIdAndOtpAsync(user.Id, request.Otp);
            otpItem.Status = OtpStatus.Consumed;
            await _otpItemService.UpdateOtpItemAsync(otpItem);

            return _jwtTokenHandler.GenerateJwtToken(user, TokenType.OtpLogin);
        }

        private async Task<User> GetCurrentUser()
        {
            var user = CurrentHttpContext.User;
            var userUid = Guid.Parse(_jwtTokenHandler.FindClaim(JwtClaims.UserUid, user));
            return await _userService.GetUserByUidAsync(userUid);
        }

        private HttpContext CurrentHttpContext => _httpContextAccessor.HttpContext;
    }
}
