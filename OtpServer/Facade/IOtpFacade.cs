using OtpServer.Dto;
using OtpServer.Request;

namespace OtpServer.Facade
{
    public interface IOtpFacade
    {
        Task<OtpItemDto> GenerateOtpAsync();

        Task<string> LoginUserWithOtpAsync(LoginWithOtpRequest request);
    }
}
