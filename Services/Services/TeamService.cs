using DataBase;
using Services.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TeamService : ITeamService
    {
        ILogger _loger;
        IDBContext _dBContext;
        public TeamService(ILogger loger, IDBContext dBContext)
        {
            _loger = loger;
            _dBContext = dBContext;
        }

        public int CreateTeam(Teams team)
        {
            try
            {
                _dBContext.Teams.Add(team);
                _dBContext.SaveChanges();
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
            return 1;
        }

        public Teams Get(int? id)
        {
            try
            {
                return _dBContext.Teams.Find(id);
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
            return null;
        }

        public void Update(Teams teams)
        {
            try
            {
                _dBContext.Entry(teams).State = EntityState.Modified;
                _dBContext.SaveChanges();
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
                throw;
            }
        }

        public void Remove(int id)
        {
            try
            {
                Teams teams = Get(id);
                _dBContext.Teams.Remove(teams);
                _dBContext.SaveChanges();
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
        }

        public IEnumerable<Teams> GetAll()
        {
            return _dBContext.Teams;
        }


    }
}
