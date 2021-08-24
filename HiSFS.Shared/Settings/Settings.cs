using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HiSFS.Shared.Settings
{
    public abstract class Settings<T>
        where T : Settings<T>, new()
    {
        private string filename;


        public Settings()
        {
        }

        public static T GetDefault()
        {
            T result = new T();
            result.SetDefault();

            result.OnLoaded();

            return result;
        }

        //public static T Get(string jsonString)
        //{
        //    var result = Deserialize(jsonString);

        //    result.OnLoaded();
        //    return result;
        //}

        public static async Task<T> LoadAsync(string filename)
        {
            T result;

            if (File.Exists(filename) == false)
            {
                result = GetDefault();
            }
            else
            {
                var jsonBytes = await File.ReadAllBytesAsync(filename);
                result = Deserialize(jsonBytes);

                result.OnLoaded();
            }

            result.filename = filename;

            return result;
        }

        public async Task SaveAsync(string filename = null)
        {
            if (filename == null)
                filename = this.filename;

            var directory = Path.GetDirectoryName(filename);
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            var jsonBytes = Serialize(this as T);
            await File.WriteAllBytesAsync(filename, jsonBytes);
        }

        /// <summary>
        /// 데이터의 기보 정보를 설정한다
        /// </summary>
        protected abstract void SetDefault();

        /// <summary>
        /// 데이터를 불러온 뒤 호출된다
        /// </summary>
        protected virtual void OnLoaded()
        {
        }

        public T Clone()
        {
            var serialized = Serialize(this as T);
            var result = Deserialize(serialized);
            result.OnLoaded();

            return result;
        }

        private static byte[] Serialize(T settings)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            // 열거형 문자 형태 지원
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(settings, options);

            return jsonBytes;
        }

        private static T Deserialize(byte[] jsonBytes)
        {
            var options = new JsonSerializerOptions
            {
            };
            // 열겨형 문자 형태 지원
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            var utf8Reader = new Utf8JsonReader(jsonBytes);
            var result = JsonSerializer.Deserialize<T>(ref utf8Reader);
            return result;
        }
    }
}
