namespace OtpServer.Repository.Model
{
    public class OtpItem : IEntity<int>
    {
        public int Id { get; set; }
        public string Otp { get; set; }
        public DateTime ExpirationDate { get; set; }
        public OtpStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool Expired => ExpirationDate.CompareTo(DateTime.Now) < 0;
    }
}
