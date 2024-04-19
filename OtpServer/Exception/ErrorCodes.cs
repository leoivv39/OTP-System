namespace OtpServer.Exception
{
    public class ErrorCodes
    {
        public static string UserAlreadyExists { get; } = "user.already.exists";
        public static string UserNotFound { get; } = "user.not.found";
        public static string InvalidOtp { get; } = "invalid.otp";
        public static string ExpiredOtp { get; } = "expired.otp";
        public static string ConsumedOtp { get; } = "consumed.otp";
    }
}
