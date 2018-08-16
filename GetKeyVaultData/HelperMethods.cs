using GetKeyVaultData.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetKeyVaultData
{
    public static class HelperMethods
    {
        public async static Task<string> GetToken(HttpClient httpClient, TokenRequestBody tokenRequestBody)
        {
            string token = string.Empty;
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Accept"))
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                }

                string grantType = tokenRequestBody.grant_type;
                string clientId = tokenRequestBody.client_id;
                string clientSecret = tokenRequestBody.client_secret;
                string scope = tokenRequestBody.scope;
                string directoryId = ConfigurationManager.AppSettings["DirectoryId"];

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("scope", scope)
                });

                string path = $"https://login.microsoftonline.com/{directoryId}/oauth2/v2.0/token";

                HttpResponseMessage response = await httpClient.PostAsync(path, formContent);
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
                    token = tokenResponse.access_token;

                }
                else
                {
                    token = string.Empty;
                }


            }
            catch (Exception ex)
            {
                token = string.Empty;
                Console.WriteLine("Error in computing token : " + ex.Message);
            }

            return token;
        }
    }

}
