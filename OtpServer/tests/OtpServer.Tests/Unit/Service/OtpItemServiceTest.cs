using Moq;
using OtpServer.Exception;
using OtpServer.Mapper.Hash;
using OtpServer.Repository;
using OtpServer.Repository.Model;
using OtpServer.Service;

namespace OtpServer.Tests.Unit.Service
{
    public class OtpItemServiceTest
    {
        private readonly IOtpItemService _otpItemService;
        private readonly Mock<IOtpItemRepository> _otpItemRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public OtpItemServiceTest()
        {
            _otpItemRepositoryMock = new Mock<IOtpItemRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _otpItemService = new OtpItemService(_otpItemRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task AddOtpItemAsync_ReturnsSavedItem()
        {
            var otpItem = GetMockOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.AddAsync(otpItem))
                .ReturnsAsync(otpItem);

            var result = await _otpItemService.AddOtpItemAsync(otpItem);

            Assert.Same(otpItem, result);
        }

        [Fact]
        public async Task GetOtpItemByUserIdAndOtpAsync_FoundNoOtpItemsForUserId_ThrowsInvalidOtpException()
        {
            var userId = 1;
            var otp = "otp";
            _otpItemRepositoryMock.Setup(repo => repo.GetAllByUserIdAsync(userId))
                .ReturnsAsync([]);

            await Assert.ThrowsAsync<InvalidOtpException>(() => _otpItemService.GetOtpItemByUserIdAndOtpAsync(userId, otp));
        }

        [Fact]
        public async Task GetOtpItemByUserIdAndOtpAsync_FoundNoItemsMatchingOtp_ThrowsInvalidOtpException()
        {
            var userId = 1;
            var otp = "otp";
            var foundOtpItem = GetMockOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.GetAllByUserIdAsync(userId))
                .ReturnsAsync([foundOtpItem]);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(otp, foundOtpItem.Otp))
                .Returns(false);

            await Assert.ThrowsAsync<InvalidOtpException>(() => _otpItemService.GetOtpItemByUserIdAndOtpAsync(userId, otp));
        }

        [Fact]
        public async Task GetOtpItemByUserIdAndOtpAsync_FoundMatchingOtpItemIsExpired_ThrowsExpiredOtpException()
        {
            var userId = 1;
            var otp = "otp";
            var foundOtpItem = GetMockExpiredOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.GetAllByUserIdAsync(userId))
                .ReturnsAsync([foundOtpItem]);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(otp, foundOtpItem.Otp))
                .Returns(true);

            await Assert.ThrowsAsync<ExpiredOtpException>(() => _otpItemService.GetOtpItemByUserIdAndOtpAsync(userId, otp));
        }

        [Fact]
        public async Task GetOtpItemByUserIdAndOtpAsync_FoundMatchingOtpItemIsConsumed_ThrowsConsumedOtpException()
        {
            var userId = 1;
            var otp = "otp";
            var foundOtpItem = GetMockConsumedOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.GetAllByUserIdAsync(userId))
                .ReturnsAsync([foundOtpItem]);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(otp, foundOtpItem.Otp))
                .Returns(true);

            await Assert.ThrowsAsync<ConsumedOtpException>(() => _otpItemService.GetOtpItemByUserIdAndOtpAsync(userId, otp));
        }

        [Fact]
        public async Task GetOtpItemByUserIdAndOtpAsync_FoundMatchingOtpItemIsValid_ReturnsFoundOtpItem()
        {
            var userId = 1;
            var otp = "otp";
            var otpItem = GetMockOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.GetAllByUserIdAsync(userId))
                .ReturnsAsync([otpItem]);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(otp, otpItem.Otp))
                .Returns(true);

            var foundOtpItem = await _otpItemService.GetOtpItemByUserIdAndOtpAsync(userId, otp);

            Assert.Same(otpItem, foundOtpItem);
        }

        [Fact]
        public async Task UpdateOtpItemAsync_ReturnsUpdatedItem()
        {
            var otpItem = GetMockOtpItem();
            var updatedItem = GetMockOtpItem();
            _otpItemRepositoryMock.Setup(repo => repo.UpdateAsync(otpItem))
                .ReturnsAsync(updatedItem);

            var result = await _otpItemService.UpdateOtpItemAsync(otpItem);

            Assert.Same(updatedItem, result);
        }

        private OtpItem GetMockOtpItem()
        {
            return new OtpItem
            {
                Otp = "otp",
                User = new User(),
                ExpirationDate = DateTime.Now.AddDays(1),
                Status = OtpStatus.Pending
            };
        }

        private OtpItem GetMockExpiredOtpItem()
        {
            return new OtpItem
            {
                ExpirationDate = DateTime.Now.AddDays(-1)
            };
        }

        private OtpItem GetMockConsumedOtpItem()
        {
            return new OtpItem
            {
                ExpirationDate = DateTime.Now.AddDays(1),
                Status = OtpStatus.Consumed
            };
        }
    }
}
