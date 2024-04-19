using OtpServer.Dto;
using OtpServer.Repository.Model;

namespace OtpServer.Mapper
{
    public interface IOtpItemMapper
    {
        OtpItem MapToPendingOtpItem(User user, string otp, DateTime expirationDate);

        OtpItemDto MapToDto(OtpItem otpItem);
    }
}
