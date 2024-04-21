using OtpServer.Exception;
using OtpServer.Mapper.Hash;
using OtpServer.Repository;
using OtpServer.Repository.Model;

namespace OtpServer.Service
{
    public class OtpItemService(IOtpItemRepository otpItemRepository, IPasswordHasher passwordHasher) : IOtpItemService
    {
        public async Task<OtpItem> AddOtpItemAsync(OtpItem otpItem)
        {
            return await otpItemRepository.AddAsync(otpItem);
        }

        public async Task<OtpItem> GetOtpItemByUserIdAndOtpAsync(int userId, string otp)
        {
            var otpItems = await otpItemRepository.GetAllByUserIdAsync(userId);
            var otpItem = otpItems.FirstOrDefault(otpItem => Matches(otpItem, otp));
            if (otpItem == null)
            {
                throw new InvalidOtpException("The otp is invalid.");
            }
            if (otpItem.Expired)
            {
                throw new ExpiredOtpException("The otp has expired.");
            }
            if (otpItem.Status == OtpStatus.Consumed)
            {
                throw new ConsumedOtpException("The otp has already been consumed.");
            }
            return otpItem;
        }

        public async Task<OtpItem> UpdateOtpItemAsync(OtpItem otpItem)
        {
            return await otpItemRepository.UpdateAsync(otpItem);
        }

        private bool Matches(OtpItem otpItem, string otp)
        {
            return passwordHasher.VerifyPassword(otp, otpItem.Otp);
        }
    }
}
