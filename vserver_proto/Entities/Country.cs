using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vserver_proto.Entities
{
    public class Country
    {
    }

    public class StateDictionary
    {     
        public List<States> Data { get; set; }
    }

    public class CityState
    {
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
    }

    [Serializable]
    public class States
    {
        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("districts")]
        public List<DistrictObj> districts { get; set; }
    }

    public class CityObj
    {
        [JsonProperty("City")]
        public string City { get; set; }
    }

    public class DistrictObj
    {
        [JsonProperty("District")]
        public string District { get; set; }

        [JsonProperty("cities")]
        public List<CityObj> Cities { get; set; }
    }
}
