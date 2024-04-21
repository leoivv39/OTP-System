using Moq;
using OtpServer.Dto;
using OtpServer.Mapper;
using OtpServer.Repository.Model;

namespace OtpServer.Tests.Unit.Mapper
{
    public class OtpItemMapperTest
    {
        private readonly OtpItemMapper _otpItemMapper;
        private readonly Mock<IUserMapper> _userMapperMock;

        public OtpItemMapperTest()
        {
            _userMapperMock = new Mock<IUserMapper>();
            _otpItemMapper = new OtpItemMapper(_userMapperMock.Object);
        }

        [Fact]
        public void MapToDto_MapsOtpItemToOtpItemDto()
        {
            var user = GetMockUser();
            var otpItem = new OtpItem
            {
                User = user,
                Otp = "test_otp",
                ExpirationDate = DateTime.Now.AddMinutes(10)
            };
            var expectedUserDto = new UserDto
            {
                Uid = user.Uid,
                Username = user.Username,
                Email = user.Email
            };
            _userMapperMock.Setup(mapper => mapper.ToDto(user))
                .Returns(expectedUserDto);

            var otpItemDto = _otpItemMapper.MapToDto(otpItem);

            Assert.Equal(expectedUserDto, otpItemDto.User);
            Assert.Equal(otpItem.Otp, otpItemDto.Otp);
            Assert.Equal(otpItem.ExpirationDate, otpItemDto.ExpirationDate);
        }

        [Fact]
        public void MapToPendingOtpItem_MapsUserOtpAndExpirationDateToOtpItem()
        {
            var user = GetMockUser();
            var otp = "test_otp";
            var expirationDate = DateTime.Now.AddMinutes(10);

            var otpItem = _otpItemMapper.MapToPendingOtpItem(user, otp, expirationDate);

            Assert.Equal(user.Id, otpItem.UserId);
            Assert.Equal(otp, otpItem.Otp);
            Assert.Equal(expirationDate, otpItem.ExpirationDate);
            Assert.Equal(OtpStatus.Pending, otpItem.Status);
        }

        private User GetMockUser()
        {
            return new User
            {
                Id = 1,
                Uid = Guid.NewGuid(),
                Username = "test_user",
                Email = "test@example.com"
            };
        }
    }
}
