namespace CachingService;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Threading.Tasks;

public class CachingService
{
    public static async Task SetData<T>(string key, T data)
    {
        using var redis = ConnectionMultiplexer.Connect("redis");
        IDatabase db = redis.GetDatabase();
        var hash = new HashEntry[] {
    new HashEntry(key, Newtonsoft.Json.JsonConvert.SerializeObject(data))
    };

        await db.HashSetAsync("searchResultChache", hash);
        redis.Close();
    }

    public static async Task<T> GetData<T>(string key)
    {
        using (var redis = ConnectionMultiplexer.Connect("redis"))
        {
            try
            {
                IDatabase db = redis.GetDatabase();
                var res = await db.HashGetAsync("searchResultChache", key);

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

