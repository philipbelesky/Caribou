namespace Caribou.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using Caribou.Properties;
    using Caribou.Forms.Models;

    public static class OSMArbitraryTypes
    {
        private static Dictionary<string, Dictionary<string, string>> KeysData;
        private static Dictionary<string, Dictionary<string, string>> ValuesData;

        public static Dictionary<string, Dictionary<string, string>> Keys
        {
            get
            {
                if (KeysData == null)
                    KeysData = GetKeys();

                return KeysData;
            }
        }

        public static Dictionary<string, Dictionary<string, string>> Values
        {
            get
            {
                if (ValuesData == null)
                    ValuesData = GetValues();

                return ValuesData;
            }
        }

        public static Dictionary<string, Dictionary<string, string>> GetKeys()
        {
            var docString = Encoding.UTF8.GetString(Resources.KeyData);
            var jsonValue = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(docString);
            var jsonDict = jsonValue.ToDictionary(item => item["key"] + "=" + item["value"]);
            return jsonDict;
        }

        public static Dictionary<string, Dictionary<string, string>> GetValues()
        {
            var docString = Encoding.UTF8.GetString(Resources.KeyValueData);
            var jsonValue = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(docString);
            var jsonDict = jsonValue.ToDictionary(item => item["key"] + "=" + item["value"]);
            return jsonDict;
        }

    }
}
