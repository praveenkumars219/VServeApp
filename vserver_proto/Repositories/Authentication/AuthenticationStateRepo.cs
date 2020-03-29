using Firebase.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using vserver_proto.Entities;

namespace vserver_proto.Repositories.Authentication
{
    public class AuthenticationStateRepo : AuthenticationStateProvider
    {
        private static ClaimsIdentity _tokenClaim;
        public static bool IsAuthenticated { get; set; }
        public static bool IsAuthenticating { get; set; }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;

            if (IsAuthenticating)
            {
                return null;
            }
            else if (IsAuthenticated)
            {
                var user = new ClaimsPrincipal(_tokenClaim);
                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public void RefreshUserAsync(UserRequest user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.Uid, "FireBaseAuth"), new[]
                                            { new Claim("token",user.TokenId), new Claim("email", user.Email),
                                                new Claim("userName", user.DisplayName) });
            _tokenClaim = identity;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Authenticate()
        {
            IsAuthenticated = true;
        }
    }
}
