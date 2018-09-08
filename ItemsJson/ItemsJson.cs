using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuestionBot.ItemsJson
{
    public class ItemsJson<T> where T : IIdentifier
    {
        private string _path = "";
        private long _currentId = 0;
        public List<T> Items { get; set; } = new List<T>();

        public ItemsJson(string path)
        {
            _path = path;
            LoadItems();
        }

        public void LoadItems()
        {
            if (File.Exists(_path))
            {
                var text = File.ReadAllText(_path);
                var result = JsonConvert.DeserializeObject<(List<T>, long)>(text);
                Items = result.Item1;
                _currentId = result.Item2;
            }
        }

        public async Task LoadItemsAsync()
        {
            if (File.Exists(_path))
            {
                var text = await File.ReadAllTextAsync(_path);
                var result = JsonConvert.DeserializeObject<(List<T>, long)>(text);
                Items = result.Item1;
                _currentId = result.Item2;
            }
        }

        public void SaveItems()
        {
            var itemsJson = JsonConvert.SerializeObject((Items, _currentId), Formatting.Indented);
            File.WriteAllText(_path, itemsJson);
        }

        public async Task SaveItemsAsync()
        {
            var itemsJson = JsonConvert.SerializeObject((Items, _currentId), Formatting.Indented);
            await File.WriteAllTextAsync(_path, itemsJson);
        }

        public long AddItem(T item)
        {
            item.Id = ++_currentId;
            Items.Add(item);
            SaveItems();
            return item.Id;
        }

        public async Task<long> AddItemAsync(T item)
        {
            item.Id = ++_currentId;
            Items.Add(item);
            await SaveItemsAsync();
            return item.Id;
        }

        public bool RemoveItem(long id)
        {
            var target = Items.SingleOrDefault(i => i.Id == id);

            if (target != null)
            {
                Items.Remove(target);
                SaveItems();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveItemAsync(long id)
        {
            var target = Items.SingleOrDefault(i => i.Id == id);

            if (target != null)
            {
                Items.Remove(target);
                await SaveItemsAsync();
                return true;
            }

            return false;
        }

        public T GetItem(long id) => Items.SingleOrDefault(i => i.Id == id);

        public bool UpdateItem(long id, T newItem)
        {
            if (RemoveItem(id))
            {
                newItem.Id = id;
                Items.Insert((int)id - 1, newItem);
                SaveItems();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateItemAsync(long id, T newItem)
        {
            if (await RemoveItemAsync(id))
            {
                newItem.Id = id;
                Items.Insert((int)id - 1, newItem);
                await SaveItemsAsync();
                return true;
            }

            return false;
        }
    }
}
