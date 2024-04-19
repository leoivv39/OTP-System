namespace OtpServer.Repository.Model
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
