using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using vserver_proto.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using vserver_proto.Helpers;
using FirebaseAdmin.Auth;

namespace vserver_proto.Data
{
    public class RegistrationService
    {
        HttpClient _client;
        private readonly IHttpContextAccessor _context;
        public RegistrationService(IHttpContextAccessor context)
        {
            _client = new HttpClient();
            _context = context;
        }

        public async Task<string> Register(UserRequest user)
        {
            try
            {
                var request = _context.HttpContext.Request;
                var reqUrl = request.Host.Value.GetUrlFromHost(request.Scheme, "register");
                _client.BaseAddress = new Uri(reqUrl);
                var response = await _client.PostJsonAsync<UserRecord>("register", user).ConfigureAwait(false);
                return response.Uid;
            }
            catch(Exception ex)
            {
                throw new Exception("User Already Exists or Unable to Add the User at this time");
            }
            
        }
    }
}
