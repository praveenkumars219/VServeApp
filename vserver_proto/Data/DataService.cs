using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using vserver_proto.Entities;
using vserver_proto.Repositories.Authentication;

namespace vserver_proto.Data
{
    public class DataService
    {
        private readonly IFireBaseRepo _fireBaseRepo;
        public DataService(IFireBaseRepo fireBaseRepo)
        {
            _fireBaseRepo = fireBaseRepo;
        }
        public async Task<IEnumerable<States>> GetStates()
        {
            JArray StatesList = await _fireBaseRepo.GetCollection("StateList", "State").ConfigureAwait(false);
            var states = JsonConvert.DeserializeObject<IList<States>>(StatesList.ToString());
            return states;
        }
    }
}
