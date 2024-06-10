using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FirstWebApp.Service
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _sessionStorageService.GetItemAsync<string>("token");
                if (string.IsNullOrEmpty(token))
                {
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }
                var jwtToken = new JwtSecurityToken(token);
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

                var user = new ClaimsPrincipal(identity);
                return await Task.FromResult(new AuthenticationState(user));
            }
            catch (Exception)
            {
                return new AuthenticationState(_anonymous);
            }
            
        }
        public void AuthenticateUser(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }
    }
}