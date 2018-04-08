using System;
using System.Collections.Generic;

namespace FantasyCup.Model
{
    public partial class User
    {
        public User()
        {
            LeagueUser = new HashSet<LeagueUser>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<LeagueUser> LeagueUser { get; set; }
    }
}
