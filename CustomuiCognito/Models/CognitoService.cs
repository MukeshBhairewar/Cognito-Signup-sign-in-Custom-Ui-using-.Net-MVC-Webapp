using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomuiCognito
{
    public interface ICognitoService
    {
        Task SignUpAsync(string username, string password, string accessKey);
        Task<SignInResponse> SignInAsync(string username, string password);
    }

    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class CognitoService : ICognitoService
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClient;
        private readonly string _userPoolId;
        private readonly string _clientId;

        public CognitoService(IAmazonCognitoIdentityProvider cognitoClient, IConfiguration configuration)
        {
            _cognitoClient = cognitoClient;
            _userPoolId = configuration["Cognito:UserPoolId"];
            _clientId = configuration["Cognito:ClientId"];
        }

        public async Task SignUpAsync(string username, string password, string activationcode)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                Username = username,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                 new AttributeType { Name = "custom:ActivationCode", Value = activationcode }
                }
            };

            var response = await _cognitoClient.SignUpAsync(signUpRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                // Successful signup
            }
            else
            {
                // Handle errors
            }
        }

        public async Task<SignInResponse> SignInAsync(string username, string password)
        {
            var signInRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                   { "USERNAME", username },
                   { "PASSWORD", password }
                },
                ClientId = _clientId
            };


            var response = await _cognitoClient.InitiateAuthAsync(signInRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return new SignInResponse
                {
                    AccessToken = response.AuthenticationResult.AccessToken,
                    IdToken = response.AuthenticationResult.IdToken,
                    RefreshToken = response.AuthenticationResult.RefreshToken
                };
            }
            else
            {
                // Handle errors
                return null;
            }
        }
    }
}
