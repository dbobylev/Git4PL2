using Git4PL2.Plugin.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding
{
    public class CheckOutObject : ICheckOutObject
    {
        [JsonProperty]
        public string ServerName { get; set; }

        [JsonProperty]
        public string Login { get; set; }

        [JsonProperty]
        public DateTime CheckoutDate { get; set; }

        [JsonProperty]
        public string ObjectOwner { get; set; }

        [JsonProperty]
        public string ObjectName { get; set; }

        [JsonProperty]
        public string ObjectType { get; set; }

        public bool Equals(ICheckOutObject other)
        {
            return ServerName == other.ServerName
                && Login == other.Login
                && ObjectOwner == other.ObjectOwner
                && ObjectName == other.ObjectName
                && ObjectType == other.ObjectType;
        }
    }
}
