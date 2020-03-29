using Firebase.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vserver_proto.Entities;

namespace vserver_proto.Repositories.Authentication
{
    public interface IFireBaseRepo
    {
        Task<UserCredential> SignInAsyc(string email, string password);
        Task<UserCredential> CreateUserAsyc(string email, string password);
        Task AddCollection(string collectionName, string collectionId);
        Task<JArray> GetCollection(string collectionName, string orderBy);
    }
}
