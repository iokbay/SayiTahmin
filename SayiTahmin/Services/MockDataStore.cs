using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SayiTahmin
{
    public class MockDataStore : IDataStore<Guess>
    {
        List<Guess> items;

        public MockDataStore()
        {
            items = new List<Guess>();
            var mockItems = new List<Guess>
            {
                new Guess("4673", 2, 1),
                new Guess("6273",1,2)
            };

            foreach (var item in mockItems)
            {
                item.IsExemined = true;
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Guess item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Guess item)
        {
            var _item = items.Where((Guess arg) => arg.sequence == item.sequence).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int sequence)
        {
            var _item = items.Where((Guess arg) => arg.sequence == sequence).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Guess> GetItemAsync(int sequence)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.sequence == sequence));
        }

        public async Task<IEnumerable<Guess>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
