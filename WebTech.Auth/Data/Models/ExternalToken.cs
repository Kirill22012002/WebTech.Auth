namespace WebTech.Auth.Data.Models;

public class ExternalToken : BaseModel
{
    public string AutorizationCode { get; set; }
    public string AccessToken { get; set; }
    public int AccessTokenExpirationSeconds { get; set; }
    public DateTime AccessTokenDateCreated { get; set; }
    public string ProviderUserId { get; set; }
    public string Provider { get; set; }
}