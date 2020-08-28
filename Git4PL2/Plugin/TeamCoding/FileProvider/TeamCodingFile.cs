using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding.FileProvider
{
    public class TeamCodingFile
    {
        [JsonProperty]
        public bool RestrickCompileWithoutCheckOut { get; set; }

        [JsonProperty]
        public IEnumerable<CheckOutObject> CheckOutObjectsList { get; set; }

        public TeamCodingFile()
        {

        }
    }
}
