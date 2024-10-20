namespace WebApi.Common.DTO.Auth
{
    public class TokenData
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
