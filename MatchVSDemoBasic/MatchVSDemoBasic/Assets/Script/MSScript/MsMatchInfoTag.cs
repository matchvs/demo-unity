using LitJson;

namespace MatchVS
{
    public class MsMatchInfoTag
    {

        public string key;
        public string value;

        public JsonData ToJson()
        {
            JsonData data = new JsonData();
            data["key"] = key;
            data["value"] = value;
            return data;
        }
    }
}

