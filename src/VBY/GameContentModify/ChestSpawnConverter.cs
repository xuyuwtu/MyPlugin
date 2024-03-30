using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VBY.Common;
using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

public class ChestSpawnConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ChestSpawnInfo[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (objectType == typeof(ChestSpawnInfo[]))
        {
            var jobject = JToken.ReadFrom(reader);
            var list = new List<ChestSpawnInfo>();

            var chestInfo = new ChestSpawnInfo();

            foreach(var info in jobject.ToArray())
            {
                var infoReader = info.CreateReader();
                serializer.Populate(info.CreateReader(), chestInfo);

                //switch (info["Action"]?.ToObject<ChestSpawnAction>() ?? ChestSpawnAction.SpawnNPC)
                switch(chestInfo.Action)
                {
                    case ChestSpawnAction.SpawnNPC:
                        {
                            var addValue = new ChestSpawnNPCInfo();
                            serializer.Populate(info.CreateReader(), addValue);
                            list.Add(addValue);
                        }
                        break;
                    case ChestSpawnAction.SpawnTile:
                        {
                            var addValue = new ChestSpawnTileInfo();
                            serializer.Populate(info.CreateReader(), addValue);
                            list.Add(addValue);
                        }
                        break;
                }
            }
            return list.ToArray();
        }
        return JsonSerializer.CreateDefault().Deserialize(reader, objectType)!;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
