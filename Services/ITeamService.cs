using DataBase;

namespace Services.Services
{
    public interface ITeamService
    {
        int CreateTeam(Teams team);
        Teams Get(int? id);
        System.Collections.Generic.IEnumerable<Teams> GetAll();
        void Remove(int id);
        void Update(Teams teams);
    }
}