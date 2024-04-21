namespace OtpServer.Dto
{
    public class OtpItemDto
    {
        public UserDto User { get; set; }
        public string Otp { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
