using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using GetKeyVaultData.DataObjects;

namespace GetKeyVaultData
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();
        static void Main(string[] args)
        {
            Console.Write("Enter secret name: ");
            string secretName = Console.ReadLine();

            Task<string> getToken = HelperMethods.GetToken(_httpClient, GetTokenRequestBody());
            getToken.Wait();
            string token = getToken.Result;

            Task<string> getSecretValue = GetSecretValue(token, secretName);
            getSecretValue.Wait();
            Console.Write("Secret key is: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(getSecretValue.Result);

            Console.ReadLine();
        }

        private async static Task<string> GetSecretValue(string token, string secretName)
        {
            string secretValue = string.Empty;
            try
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                if (!_httpClient.DefaultRequestHeaders.Contains("Accept"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }

                //Compose the request URI - for more info see https://docs.microsoft.com/en-us/rest/api/keyvault/getsecret/getsecret
                string path = $"https://{ConfigurationManager.AppSettings["KeyVaultName"]}.vault.azure.net/secrets/{secretName}?api-version=2016-10-01";
                
                HttpResponseMessage response = await _httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    KeyVaultResponse secretValueResponse = await response.Content.ReadAsAsync<KeyVaultResponse>();
                    secretValue = secretValueResponse.value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getting the secret : " + ex.Message);
            }

            return secretValue;
        }

        private static TokenRequestBody GetTokenRequestBody()
        {
            TokenRequestBody tokenRequestBody = new TokenRequestBody();
            tokenRequestBody.grant_type = ConfigurationManager.AppSettings["GrantType"];
            tokenRequestBody.client_id = ConfigurationManager.AppSettings["ClientId"];
            tokenRequestBody.client_secret = ConfigurationManager.AppSettings["ClientSecret"];
            tokenRequestBody.scope = ConfigurationManager.AppSettings["Scope"];
            return tokenRequestBody;
        }
    }
    
}
