using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;
using UserCredential = Firebase.Auth.UserCredential;
using vserver_proto.Entities;

namespace vserver_proto.Repositories.Authentication
{
    public class FireBaseRepo : IFireBaseRepo
    {
        private static FirebaseAuthClient _client;
        private static IConfiguration _config;
        private static FirestoreDb db;
        public FireBaseRepo(IConfiguration config)
        {
            _config = config;
            var _fieConfig = new FirebaseAuthConfig
            {
                ApiKey = _config.GetValue<string>("FirebaseConfig:ApiKey"),
                AuthDomain = _config.GetValue<string>("FirebaseConfig:AuthDomain"),
                Providers = new FirebaseAuthProvider[] { new GoogleProvider().AddScopes("email"), new EmailProvider() },
                UserRepository = new FileUserRepository("FirebaseUser"),
            };

            _client = new FirebaseAuthClient(_fieConfig);
            CreateDatabase();
        }

        public async Task<UserCredential> SignInAsyc(string email, string password)
        {
            return await _client.SignInWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
        }

        public async Task<UserCredential> CreateUserAsyc(string email, string password)
        {
            return await _client.CreateUserWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
        }

        public static void CreateDatabase()
        {
            //string path = _config.GetValue<string>("FirebaseConfig:ProjectId");
            //object auth = AuthExplicit(path);
            //db = FirestoreDb.Create(_config.GetValue<string>("FirebaseConfig:ProjectId"));

        }
        public static object AuthExplicit(string projectId)
        {
            var credential = GoogleCredential.FromFile(@"C:\Users\Pkuma\Documents\Personal\Trevista\vserve-fc2042b5ca27.json");
            var storage = StorageClient.Create(credential);
            var buckets = storage.ListBuckets(projectId);
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }

        public async Task AddCollection(string collectionName, string jsonCollection)
        {
            FirebaseClient firebase = GetFirebaseClient();
            var dino = await firebase.Child(collectionName).PostAsync(jsonCollection);
        }

        private FirebaseClient GetFirebaseClient()
        {
            return new FirebaseClient(_config.GetValue<string>("FirebaseConfig:DatabaseURL"), new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(_config.GetValue<string>("FirebaseConfig:SecretKey"))
            });
        }

        public async Task<JArray> GetCollection(string collectionName, string orderBy)
        {
            FirebaseClient firebase = GetFirebaseClient();
            JArray dataCollection = null;
           // await AddCollection(collectionName, jsonObject().ToString()).ConfigureAwait(false);
            var test = await firebase.Child(collectionName).OnceAsync<JArray>().ConfigureAwait(false);
            //.AsObservable<States>().Subscribe(x=> x)
            foreach (var item in test)
            {
                var s = item.Key;
                dataCollection = JArray.FromObject(item.Object);
            }
            return dataCollection;
        }

        public JArray jsonObject()
        {
            //JObject o1 = JObject.Parse(File.ReadAllText(@"C:\Users\Pkuma\Desktop\cities.json"));
            JArray o2;
            using (StreamReader file = File.OpenText(@"C:\Users\Pkuma\Desktop\cities.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                o2 = (JArray)JToken.ReadFrom(reader);
            }

            return o2;
        }
    }
}
