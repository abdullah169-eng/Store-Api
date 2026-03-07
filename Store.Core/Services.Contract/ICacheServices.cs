using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface ICacheServices
    {
        Task SetCachekeyAsync(string key, object value, TimeSpan expiration);
        Task<string> GetCacheKeyAsync(string key);
    }
}
