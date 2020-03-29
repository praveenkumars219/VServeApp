using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Auth0.ManagementApi.Models.Rules;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using vserver_proto.Data;
using vserver_proto.Entities;
using vserver_proto.Repositories.Authentication;

namespace vserver_proto.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        // private readonly UserStore userStore = new UserStore();
        private LoginService _loginService;
        private readonly IConfiguration _config;
        private readonly IFireBaseRepo _fireBaseRepo;
        public LoginController(LoginService loginService, IFireBaseRepo fireBaseRepo)
        {
            _loginService = loginService;
            _fireBaseRepo = fireBaseRepo;
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel request)
        {
            try
            {
                //var user = await this.userStore.LoginAsync(request);
                var token = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(request.UserId).ConfigureAwait(false);
                // var user = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserByEmailAsync(request.UserId).ConfigureAwait(false);
                // var token = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(request.UserId).ConfigureAwait(false);
                UserCredential user = await _fireBaseRepo.SignInAsyc(request.UserId, request.Password).ConfigureAwait(false);
                await _fireBaseRepo.AddCollection("states", "India").ConfigureAwait(false);
                UserRequest userRequest = new UserRequest
                {
                    DisplayName = user.User.Info.DisplayName,
                    Email = user.User.Info.Email,
                    PhoneNumber = user.User.Info.FederatedId,
                    PhotoUrl = user.User.Info.PhotoUrl,
                    TokenId = user.User.Credential.IdToken,
                    Uid = user.User.Uid,
                    Password = request.Password,
                };
                return Ok(userRequest);
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }
    }
}