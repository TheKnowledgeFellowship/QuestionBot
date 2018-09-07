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
        private int _currentId = 0;
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
                var result = JsonConvert.DeserializeObject<(List<T>, int)>(text);
                Items = result.Item1;
                _currentId = result.Item2;
            }
        }

        public async Task LoadItemsAsync()
        {
            if (File.Exists(_path))
            {
                var text = await File.ReadAllTextAsync(_path);
                var result = JsonConvert.DeserializeObject<(List<T>, int)>(text);
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

        public void AddItem(T item)
        {
            item.Id = ++_currentId;
            Items.Add(item);
            SaveItems();
        }

        public async Task AddItemAsync(T item)
        {
            item.Id = ++_currentId;
            Items.Add(item);
            await SaveItemsAsync();
        }

        public bool RemoveItem(int id)
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

        public async Task<bool> RemoveItemAsync(int id)
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

        public T GetItem(int id) => Items.SingleOrDefault(i => i.Id == id);

        public bool UpdateItem(int id, T newItem)
        {
            if (RemoveItem(id))
            {
                newItem.Id = id;
                AddItem(newItem);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateItemAsync(int id, T newItem)
        {
            if (await RemoveItemAsync(id))
            {
                newItem.Id = id;
                await AddItemAsync(newItem);
                return true;
            }

            return false;
        }
    }
}
