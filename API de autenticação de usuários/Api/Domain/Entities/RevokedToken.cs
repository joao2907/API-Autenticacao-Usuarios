namespace Api.Domain.Entities
{
    public class RevokedToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
