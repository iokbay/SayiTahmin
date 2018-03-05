using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace SayiTahmin
{
    public class CloudDataStore : IDataStore<Guess>
    {
        HttpClient client;
        IEnumerable<Guess> items;

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.BackendUrl}/");

            items = new List<Guess>();
        }

        public async Task<IEnumerable<Guess>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Guess>>(json));
            }

            return items;
        }

        public async Task<Guess> GetItemAsync(int sequence)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var json = await client.GetStringAsync($"api/item/{sequence}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Guess>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Guess item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Guess item)
        {
            if (item == null || !CrossConnectivity.Current.IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.sequence}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(int sequence)
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/item/{sequence}");

            return response.IsSuccessStatusCode;
        }
    }
}
