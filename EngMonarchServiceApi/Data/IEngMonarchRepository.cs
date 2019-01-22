using EngMonarchApi.Models;
using System.Threading.Tasks;

namespace EngMonarchApi.Data
{
    public interface IEngMonarchRepository
    {
        Task<PagedList<EngMonarch>> GetPageAsync(int page = 1, int pageSize = 10);
        Task<EngMonarch> GetByIdAsync(string id);
        void CreateAsync(EngMonarch engmonarch);
        void DeleteAsync(string id);
        void UpdateAsync(EngMonarch engmonarch);
    }
}
