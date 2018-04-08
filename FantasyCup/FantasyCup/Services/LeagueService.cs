using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyCup.Model;
using FantasyCup.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FantasyCup.Services
{
    public interface ILeagueService
    {
        League Create(League league);
        League Update(League leagueParam, int userId);
        void Delete(int leagueId, int userId);
        IEnumerable<League> GetAll();
        IEnumerable<League> GetLeaguesByUserId(int userId);
        League Find(string name);
        League Join(int leagueId, int userId, int code, LeagueJoinMode mode);
        void Leave(int leagueId, int userId);
        League GetDefaultLeague();
        League GetLeagueById(int leagueId, int userId);
        IEnumerable<LeagueUser> GetAllMembers(int leagueId, int userId);
        void UpdateMembers(int leagueId, int userId, LeagueUser[] members);
    }

    public class LeagueService :ILeagueService
    {
        private FantasyCupContext _context;

        public LeagueService(FantasyCupContext context)
        {
            _context = context;
        }

        public League Create(League league)
        {
            if (_context.League.Any(l => l.Name == league.Name))
                throw new FantasyException("A league with that name already exists");

            league.Code = GenerateUniqueLeagueCode();
            league.Created = DateTime.Now;

            _context.League.Add(league);
            _context.SaveChanges();

            return league;
        }

        public League Update(League leagueParam, int userId)
        {
            League league = _context.League.Find(leagueParam.Id);
            if (league == null)
                throw new FantasyException("League not found");

            LeagueUser lu = _context.LeagueUser.Find(league.Id, userId);

            if (!lu.IsAdmin)
                throw new FantasyException("Only the administrator of this league can update a league's properties.");

            if (league.Name != leagueParam.Name)
            {
                if (_context.League.Any(l => l.Name == leagueParam.Name))
                    throw new FantasyException("A league with that name already exists");
            }

            league.Name = leagueParam.Name;
            league.HasPotMoney = leagueParam.HasPotMoney;
            league.PotAmount = leagueParam.PotAmount;

            _context.League.Update(league);
            _context.SaveChanges();

            return league;                
        }

        public void Delete(int leagueId, int userId)
        {
            var league = _context.League.Find(leagueId);
            if (league == null)
                throw new FantasyException("League not found");
            if (!_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are not a member of this league");

            LeagueUser lu = _context.LeagueUser.Find(leagueId, userId);

            if (!lu.IsAdmin)
                throw new FantasyException("Only the administrator of this league can delete it.");
            if (_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId != userId))
                throw new FantasyException("The league can't be deleted while there are other members in it.");

            _context.League.Remove(league);
            _context.SaveChanges();

        }

        public League Find(string name)
        {
            if (_context.League.Any(x => x.Name == name))
                return _context.League.Where(x => x.Name == name).Single();
            else
                throw new FantasyException("No such league exists");
        }

        public League Join(int leagueId, int userId, int code, LeagueJoinMode mode)
        {
            var league = _context.League.Find(leagueId);
            if (league == null)
                throw new FantasyException("League not found");

            var user = _context.User.Find(userId);
            if (user == null)
                throw new FantasyException("User not found");

            if (league.Code != code)
                throw new FantasyException("The entered code is incorrect for this league");

            if (_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are already a member of this league");

            LeagueUser member = new LeagueUser();
            member.LeagueId = leagueId;
            member.UserId = userId;
            member.IsAdmin = (mode == LeagueJoinMode.Create);
            member.JoinDate = DateTime.Now;
            member.Paid = false;

            _context.LeagueUser.Add(member);
            _context.SaveChanges();

            return league;

        }

        public IEnumerable<League> GetAll()
        {
            return _context.League;
        }

        public IEnumerable<League> GetLeaguesByUserId(int userId)
        {
            return _context.League.Where(x => x.LeagueUser.Select(y => y.UserId).Contains(userId));
        }

        public League GetDefaultLeague()
        {
            return _context.League.Where(x => x.Name == "Overall").Single();
        }

        public League GetLeagueById(int leagueId, int userId)
        {
            var league = _context.League.Find(leagueId);
            if (league == null)
                throw new FantasyException("League not found");

            if (!_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are not a member of this league");

            return league;
        }

        public void Leave(int leagueId, int userId)
        {
            var league = _context.League.Find(leagueId);
            if (league == null)
                throw new FantasyException("League not found");

            var user = _context.User.Find(userId);
            if (user == null)
                throw new FantasyException("User not found");

            if (!_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are not a member of this league");

            if (leagueId == GetDefaultLeague().Id)
                throw new FantasyException("You can't leave the default league");

            LeagueUser lu = _context.LeagueUser.Find(leagueId, userId);
            if (lu.Paid)
                throw new FantasyException("You have already paid the pot money. You shouldn't leave.");

            if (lu.IsAdmin)
                throw new FantasyException("You are the administrator of this league. You are too important to leave.");

            _context.LeagueUser.Remove(lu);
            _context.SaveChanges();
        }

        public IEnumerable<LeagueUser> GetAllMembers(int leagueId, int userId)
        {
            if (!_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are not a member of this league");

            return _context.LeagueUser.Include(u => u.User).Where(x => x.LeagueId == leagueId);
        }

        public void UpdateMembers(int leagueId, int userId, LeagueUser[] members)
        {
            var league = _context.League.Find(leagueId);
            if (league == null)
                throw new FantasyException("League not found");
            if (!_context.LeagueUser.Any(x => x.LeagueId == leagueId && x.UserId == userId))
                throw new FantasyException("You are not a member of this league");

            LeagueUser lu = _context.LeagueUser.Find(leagueId, userId);

            if (!lu.IsAdmin)
                throw new FantasyException("Only the administrator of this league can update its members.");

            _context.Entry(lu).State = EntityState.Detached;

            foreach(var member in members)
            {
                var memberToUpdate = _context.LeagueUser.Find(member.LeagueId, member.UserId);
                memberToUpdate.Paid = member.Paid;
                _context.LeagueUser.Update(memberToUpdate);
            }
            _context.SaveChanges();
        }

        private int GenerateUniqueLeagueCode()
        {
            Random generator = new Random();
            int code = generator.Next(100000, 999999);

            while(_context.League.Any(l => l.Code == code))
            {
                code = generator.Next(100000, 999999);
            }

            return code;
        }
    }
}
