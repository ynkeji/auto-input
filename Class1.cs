// 需要在NuGet里面添加 Newtonsoft.Json

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonModel
{

    public class TestModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("updatainformation")]
        public string Updatainformation { get; set; }
    }

}
