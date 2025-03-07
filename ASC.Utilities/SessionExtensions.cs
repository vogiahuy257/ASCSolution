using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ASC.Utilities
{
    public static class SessionExtensions
    {
        public static void SetSession(this ISession session, string key, object value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            session.Set(key, Encoding.UTF8.GetBytes(jsonString)); // Dùng UTF8 để tránh lỗi ký tự
        }

        public static T? GetSession<T>(this ISession session, string key)
        {
            if (session.TryGetValue(key, out byte[] value))
            {
                var jsonString = Encoding.UTF8.GetString(value);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return default; // Trả về null nếu key không tồn tại
        }
    }
}
