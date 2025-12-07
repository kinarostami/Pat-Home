using System;
using System.Threading.Tasks;

namespace Common.Cache
{
    public interface IMemoryCacheHelper
    {
        Task<T> GetOrSet<T>(string key, Func<Task<T>> func);
        void RemoveCache(string key);
    }
}