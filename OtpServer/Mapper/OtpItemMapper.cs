using OtpServer.Dto;
using OtpServer.Repository.Model;

namespace OtpServer.Mapper
{
    public class OtpItemMapper : IOtpItemMapper
    {
        private readonly IUserMapper _userMapper;

        public OtpItemMapper(IUserMapper userMapper)
        {
            _userMapper = userMapper;
        }

        public OtpItemDto MapToDto(OtpItem otpItem)
        {
            return new OtpItemDto
            {
                User = _userMapper.ToDto(otpItem.User),
                Otp = otpItem.Otp,
                ExpirationDate = otpItem.ExpirationDate
            };
        }

        public OtpItem MapToPendingOtpItem(User user, string otp, DateTime expirationDate)
        {
            return new OtpItem
            {
                UserId = user.Id,
                Otp = otp,
                ExpirationDate = expirationDate,
                Status = OtpStatus.Pending
            };
        }
    }
}
