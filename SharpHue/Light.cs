using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHue
{
    public class Light
    {
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
