
namespace GetKeyVaultData.DataObjects
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

    public class KeyVaultResponse
    {
        public string value { get; set; }
        public string id { get; set; }
    }

    public class TokenRequestBody
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
    }
}
