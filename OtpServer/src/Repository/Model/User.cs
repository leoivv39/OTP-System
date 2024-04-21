using System.ComponentModel.DataAnnotations.Schema;

namespace OtpServer.Repository.Model
{
    [Table("Users")]
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
