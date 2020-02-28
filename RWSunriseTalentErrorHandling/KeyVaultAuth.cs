using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace TalentErrorHandling
{
    internal static class KeyVaultAuth
    {
        internal static string GetSecretFromVault(string keyName)
        {
            string secretUri = Constants.GetEnvironmentVariable(keyName);
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetSecretToken));
            var passwordSecret = keyVault.GetSecretAsync(secretUri).GetAwaiter().GetResult();

            return passwordSecret.Value;
        }

        private static async Task<string> GetSecretToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(Constants.GetEnvironmentVariable("ClientID_KeyVault"), Constants.GetEnvironmentVariable("ClientSecret_KeyVault"));
            AuthenticationResult authResult = await authContext.AcquireTokenAsync(resource, clientCred);

            if (authResult == null)
                throw new InvalidOperationException("Unable to obtain required token");

            return authResult.AccessToken;
        }
    }
}
