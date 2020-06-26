using System.Collections.Generic;
using DataBase;

namespace Services.Services
{
    public interface IMatchService
    {
        int Create(Matches match);
        Matches Get(int? id);
        IEnumerable<Matches> GetAll();
        void Remove(int id);
        void Update(Matches match);
    }
}