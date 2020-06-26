using DataBase;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MatchService : IMatchService
    {
        const int PointsForDraw = 1;
        const int PointsForWin = 3;
        const int PointsForLoss = 0;

        ILogger _loger;
        IDBContext _dBContext;
        public MatchService(ILogger loger, IDBContext dBContext)
        {
            _loger = loger;
            _dBContext = dBContext;
        }

        public int Create(Matches match)
        {
            try
            {
                _dBContext.Matches.Add(match);
                _dBContext.SaveChanges();
                UpdateWinStatus(match);
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
            return 1;
        }

        private void UpdateWinStatus(Matches match)
        {
            if(match.ResultTeam1 == match.ResultTeam2)
            {
                AddPointsToTeam(match.TeamId1, PointsForDraw);
                AddPointsToTeam(match.TeamId2, PointsForDraw);
            }
            else if (match.ResultTeam1 > match.ResultTeam2)
            {
                AddPointsToTeam(match.TeamId1, PointsForWin);
                AddPointsToTeam(match.TeamId2, PointsForLoss);
            }
            else
            {
                AddPointsToTeam(match.TeamId1, PointsForLoss);
                AddPointsToTeam(match.TeamId2, PointsForWin);
            }
        }

        private void AddPointsToTeam(int teamId, int pointsToAdd)
        {
            Teams team = _dBContext.Teams.Find(teamId);
            team.Result += pointsToAdd;
            _dBContext.Entry(team).State = EntityState.Modified;
            _dBContext.SaveChanges();
        }

        public Matches Get(int? id)
        {
            try
            {
                return _dBContext.Matches.Find(id);
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
            return null;
        }

        public void Update(Matches match)
        {
            try
            {
                _dBContext.Entry(match).State = EntityState.Modified;
                _dBContext.SaveChanges();
                _dBContext.RefreshResults();
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
                Matches match = Get(id);
                _dBContext.Matches.Remove(match);
                _dBContext.SaveChanges();
                _dBContext.RefreshResults();
            }
            catch (Exception e)
            {
                _loger.Log(e.Message);
            }
        }

        public IEnumerable<Matches> GetAll()
        {
            //var matches = _dBContext.Matches.Include(m => m.Teams).Include(m => m.Teams1);
            return _dBContext.Matches.Include(m => m.Teams).Include(m => m.Teams1);
        }

    }
}
