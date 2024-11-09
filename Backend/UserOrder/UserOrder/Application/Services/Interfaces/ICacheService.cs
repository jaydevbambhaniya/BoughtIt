using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Application.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key);
        Task<bool> Set<T>(string key, T value,int expiryMinutes);
        Task<bool> Delete(string key);
    }
}
