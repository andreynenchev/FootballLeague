using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBase;
using Services.Services;

namespace FootballLeague.Controllers
{
    public class MatchesController : Controller
    {
        IMatchService _matchService;
        ITeamService _teamService;

        public MatchesController(IMatchService matchService, ITeamService teamService)
        {
            _matchService = matchService;
            _teamService = teamService;
        }


        // GET: Matches
        public ActionResult Index()
        {
            var matches = _matchService.GetAll();
            return View(matches.ToList());
        }

        // GET: Matches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = _matchService.Get(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            return View(matches);
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            ViewBag.TeamId1 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName");
            ViewBag.TeamId2 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MatchId,TeamId1,TeamId2,ResultTeam1,ResultTeam2")] Matches matches)
        {
            if (ModelState.IsValid)
            {
                _matchService.Create(matches);
                return RedirectToAction("Index");
            }

            ViewBag.TeamId1 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId1);
            ViewBag.TeamId2 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId2);
            return View(matches);
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = _matchService.Get(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId1 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId1);
            ViewBag.TeamId2 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId2);
            return View(matches);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchId,TeamId1,TeamId2,ResultTeam1,ResultTeam2")] Matches matches)
        {
            if (ModelState.IsValid)
            {
                _matchService.Update(matches);
                return RedirectToAction("Index");
            }
            ViewBag.TeamId1 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId1);
            ViewBag.TeamId2 = new SelectList(_teamService.GetAll(), "TeamId", "TeamName", matches.TeamId2);
            return View(matches);
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = _matchService.Get(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            return View(matches);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _matchService.Remove(id);
            return RedirectToAction("Index");
        }


    }
}
