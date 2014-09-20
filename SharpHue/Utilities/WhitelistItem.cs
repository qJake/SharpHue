using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpHue.Utilities
{
    public class WhitelistItem
    {
        //TODO Add DateTime
        public WhitelistItem(string createDate, string lastUseDate, string name)
        {
            CreateDate = createDate;
            LastUseDate = lastUseDate;
            Name = name;
        }

        public WhitelistItem(string id, string createDate, string lastUseDate, string name)
        {
            ID = id;
            CreateDate = createDate;
            LastUseDate = lastUseDate;
            Name = name;
        }

        public WhitelistItem()
        {

        }

        public string ID { get; set; }

        [JsonProperty("create date")]
        //        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
        public string CreateDate { get; set; }
        [JsonProperty("last use date")]
        public string LastUseDate { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region Management Methods

        public bool Delete(string currentUsername)
        {
            bool result;

            try
            {
                var jObject = JsonClient.Request(HttpMethod.Delete, string.Format("/api/{0}/config/whitelist/{1}", currentUsername, ID)) as JObject;
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #endregion
    }
}
