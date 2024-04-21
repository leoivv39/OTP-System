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
    public class OtpFacade(
        IOtpItemService otpItemService,
        IHttpContextAccessor httpContextAccessor,
        IJwtTokenHandler jwtTokenHandler,
        IUserService userService,
        IOtpProvider otpProvider,
        IOtpItemMapper otpItemMapper,
        IPasswordHasher passwordHasher)
        : IOtpFacade
    {
        public async Task<OtpItemDto> GenerateOtpAsync()
        {
            var foundUser = await GetCurrentUser();

            string generatedOtp = otpProvider.GenerateTotp(out var expirationDate);
            string hashedOtp = passwordHasher.HashPassword(generatedOtp);

            OtpItem otpItem = otpItemMapper.MapToPendingOtpItem(foundUser, hashedOtp, expirationDate);
            OtpItem savedOtpItem = await otpItemService.AddOtpItemAsync(otpItem);
            savedOtpItem.Otp = generatedOtp;
            return otpItemMapper.MapToDto(savedOtpItem);
        }

        public async Task<string> LoginUserWithOtpAsync(LoginWithOtpRequest request)
        {
            var user = await GetCurrentUser();
            var otpItem = await otpItemService.GetOtpItemByUserIdAndOtpAsync(user.Id, request.Otp);
            otpItem.Status = OtpStatus.Consumed;
            await otpItemService.UpdateOtpItemAsync(otpItem);

            return jwtTokenHandler.GenerateJwtToken(user, TokenType.OtpLogin);
        }

        private async Task<User> GetCurrentUser()
        {
            var user = CurrentHttpContext.User;
            var userUid = Guid.Parse(jwtTokenHandler.FindClaim(JwtClaims.UserUid, user));
            return await userService.GetUserByUidAsync(userUid);
        }

        private HttpContext CurrentHttpContext => httpContextAccessor.HttpContext;
    }
}
