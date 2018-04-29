using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class User
    {
        public User()
        {
            CompetitionUserBet = new HashSet<CompetitionUserBet>();
            GameUserBet = new HashSet<GameUserBet>();
            LeagueUser = new HashSet<LeagueUser>();
            UserBet = new HashSet<UserBet>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<CompetitionUserBet> CompetitionUserBet { get; set; }
        public ICollection<GameUserBet> GameUserBet { get; set; }
        public ICollection<LeagueUser> LeagueUser { get; set; }
        public ICollection<UserBet> UserBet { get; set; }
    }
}
