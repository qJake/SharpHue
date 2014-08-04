using Newtonsoft.Json;

namespace SharpHue.Utilities
{
    public class WhitelistItem
    {
        //TODO Added ID if possible
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
    }
}
