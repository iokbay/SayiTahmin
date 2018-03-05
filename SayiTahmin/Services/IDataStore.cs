using System.Collections.Generic;
using System.Threading.Tasks;

namespace SayiTahmin
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int sequence);
        Task<T> GetItemAsync(int sequence);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
