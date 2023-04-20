namespace CachingService;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

public class CachingService
{
    public static void SetData<T>(string key, T data)
    {
        using var redis = ConnectionMultiplexer.Connect("redis");
        IDatabase db = redis.GetDatabase();
        db.StringSet(key, Newtonsoft.Json.JsonConvert.SerializeObject(data));
        redis.Close();
    }

    public static T GetData<T>(string key)
    {
        using (var redis = ConnectionMultiplexer.Connect("redis"))
        {
            try
            {
                IDatabase db = redis.GetDatabase();
                var res = db.StringGet(key);

                redis.Close();
                if (res.IsNull)
                    return default(T);
                
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }
            catch
            {
                return default(T);
            }
        }

    }
}

