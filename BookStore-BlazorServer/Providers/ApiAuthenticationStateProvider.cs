using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreUI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JwtSecurityTokenHandler _jwt;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorage,
                                              JwtSecurityTokenHandler jwt)
        {
            _localStorage = localStorage;
            _jwt = jwt;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                //var tokenContent = await _localStorage.GetItemAsync<JwtSecurityToken>("authToken");
                //if(tokenContent == null)
                //{
                //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                //}

                var savedToken = await _localStorage.GetItemAsStringAsync("authToken");
                if(string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var tokenContent = _jwt.ReadJwtToken(savedToken);                

                var expiry = tokenContent.ValidTo;
                if(expiry < DateTime.Now)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Get Claims from token and Build auth user object
                var claims = ParseClaims(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

                //return authenticated person
                return new AuthenticationState(user);              
            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task LoggedIn()
        {
            var savedToken = await _localStorage.GetItemAsStringAsync("authToken");            
            var tokenContent = _jwt.ReadJwtToken(savedToken);
            //var tokenContent2 = await _localStorage.GetItemAsync<JwtSecurityToken>("authToken");
            var claims = ParseClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);

        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
