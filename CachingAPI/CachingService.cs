namespace CachingService;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Threading.Tasks;

public class CachingService
{
    public static async Task SetData<T>(string hashKey, string key, T data)
    {
        using var redis = ConnectionMultiplexer.Connect("redis");
        IDatabase db = redis.GetDatabase();
        var hash = new HashEntry[] {
    new HashEntry(key, Newtonsoft.Json.JsonConvert.SerializeObject(data))
    };

        await db.HashSetAsync(hashKey, hash);
        redis.Close();
    }

    public static async Task<T> GetData<T>(string hashKey, string key)
    {
        using (var redis = ConnectionMultiplexer.Connect("redis"))
        {
            try
            {
                IDatabase db = redis.GetDatabase();
                var res = await db.HashGetAsync(hashKey, key);

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
    
    public static async Task<T[]> GetWholeHash<T>(string hashKey)
    {
        {
            using (var redis = ConnectionMultiplexer.Connect("redis"))
            {
                try
                {
                    IDatabase db = redis.GetDatabase();
                    var res = await db.HashGetAllAsync(hashKey);

                    redis.Close();
                    if (res == null)
                        return default(T[]);

                    //Deserialize the hashEntry array into an array of T
                    var resDeserialized = new List<T>();
                    foreach (var item in res)
                    {
                        resDeserialized.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.Value));

                    }
                    return resDeserialized.ToArray();
                }
                catch (Exception e)
                {
                    return default(T[]);
                }
            }

        }
    }
}

