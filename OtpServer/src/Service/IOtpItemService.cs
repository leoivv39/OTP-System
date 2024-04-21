using OtpServer.Repository.Model;

namespace OtpServer.Service
{
    public interface IOtpItemService
    {
        Task<OtpItem> AddOtpItemAsync(OtpItem otpItem);

        Task<OtpItem> GetOtpItemByUserIdAndOtpAsync(int userId, string otp);

        Task<OtpItem> UpdateOtpItemAsync(OtpItem otpItem);
    }
}
