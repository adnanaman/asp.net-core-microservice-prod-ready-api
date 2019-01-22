using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngMonarchApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace EngMonarchApi.Data
{
    public class EngMonarchRepository : IEngMonarchRepository
    {
        private IList<EngMonarch> _engmonarchs;

        public async Task<PagedList<EngMonarch>> GetPageAsync(int page = 1, int pageSize = 10)
        {
            var engmonarchs = await GetEngMonarchs();

            return new PagedList<EngMonarch>
            {
                Items = engmonarchs.Skip((page - 1) * pageSize).Take(pageSize),
                Page = page,
                PageSize = pageSize,
                TotalCount = engmonarchs.Count
            };
        }

        public async Task<EngMonarch> GetByIdAsync(string id)
        {
            var engmonarchs = await GetEngMonarchs();

            return engmonarchs.FirstOrDefault(x => x.Id == id);
        }

        public async void CreateAsync(EngMonarch engmonarch)
        {
            engmonarch.Id = await GetNextIdAsync();
            var engmonarchs = await GetEngMonarchs();
            engmonarchs.Add(engmonarch);
        }

        public async void DeleteAsync(string id)
        {
            var engmonarchs = await GetEngMonarchs();
            var engmonarchToDelete = engmonarchs.First(x => x.Id == id);

            engmonarchs.Remove(engmonarchToDelete);
        }

        public async void UpdateAsync(EngMonarch engmonarch)
        {
            var engmonarchs = await GetEngMonarchs();
            var engmonarchToUpdate = engmonarchs.First(x => x.Id == engmonarch.Id);

            engmonarchs.Remove(engmonarchToUpdate);
            engmonarchs.Add(engmonarch);
        }

        private async Task<IList<EngMonarch>> GetEngMonarchs()
        {
            if (_engmonarchs == null)
            {                
                var assembly = typeof(EngMonarchRepository).Assembly;
                var resourceStream = assembly.GetManifestResourceStream("EngMonarchApi.Data.englishMonarchs.json");

                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var engmonarchsArray = (await JObject.LoadAsync(jsonReader))["engmonarchs"];
                    _engmonarchs = engmonarchsArray.ToObject<IList<EngMonarch>>();
                }
            }

            return _engmonarchs;
        }

        private async Task<string> GetNextIdAsync()
        {
            var engmonarchs = await GetEngMonarchs();
            var maxId = engmonarchs.Select(x => int.Parse(x.Id.Split("-")[1])).Max();
            return $"gam-{maxId + 1:D6}";
        }
    }
}
