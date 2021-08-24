using Microsoft.Extensions.Configuration;
using System;

namespace HiSFS.WebApp.Services
{
    public class StorageService : IDisposable
    {
        public StorageService(IConfiguration config)
        {
            var section = config.GetSection("RemoteStorage");
            var rootPath = section["Root"];
            //
        }

        public void Dispose()
        {
        }
    }
}