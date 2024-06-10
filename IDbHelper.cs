using FirstWebApp.Components.Pages;
using System.Threading.Tasks;

namespace FirstWebApp.Service
{
    public interface IDbHelper
    {
        public Task <List<T>> Fetch<T>(string command);
        public void Save(string command);
        Task InsertElectricalChecklistAsync(ElectricalChecklistModel model, List<ChecklistItem> ChecklistItemsA, List<ChecklistItem> ChecklistItemsB,
            List<ChecklistItem> ChecklistItemsC, List<ChecklistItem> ChecklistItemsD, List<ChecklistItem> ChecklistItemsE, List<ChecklistItem> ChecklistItemsF);

        Task AddUser(User user);
        Task<User> GetUserByEmailOrName(string emailOrName);

        Task ExecuteRawSqlAsync(string sql, object parameters);
    }
}
