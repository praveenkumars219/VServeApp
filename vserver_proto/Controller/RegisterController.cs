using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auth0.ManagementApi.Models.Rules;
using Auth0.ManagementApi.Models;
using FirebaseAdmin.Auth;
using System.Net.Http;
using vserver_proto.Entities;

namespace vserver_proto.Controller
{   
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        public async Task<UserRecord> Register(UserRecordArgs userOrgs)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userOrgs).ConfigureAwait(false);
                return userRecord;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}