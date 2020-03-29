using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using vserver_proto.Entities;
using Microsoft.AspNetCore.Components;
using Auth0.ManagementApi.Models.Rules;
using Microsoft.AspNetCore.Http;
using vserver_proto.Helpers;
using Firebase.Auth;

namespace vserver_proto.Data
{
    public class LoginService
    {
        private readonly IHttpContextAccessor _context;
        HttpClient _client;
        public LoginService(IHttpContextAccessor context)
        {
            _client = new HttpClient();
            _context = context;
        }

        public async Task<UserRequest> Login(LoginModel login)
        {
            try
            {
                var request = _context.HttpContext.Request;
                var reqUrl = request.Host.Value.GetUrlFromHost(request.Scheme,null);
                _client.BaseAddress = new Uri(reqUrl);
                var token = await _client.PostJsonAsync<UserRequest>("login", login).ConfigureAwait(false);
                return token;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
