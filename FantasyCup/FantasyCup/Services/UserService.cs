using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyCup.Model;
using FantasyCup.Helpers;

namespace FantasyCup.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User Create(User user, string password, string refreshToken);
        void SaveRefreshToken(int userId, string token);
        bool VerifyRefreshToken(int userId, string token);
    }

    public class UserService : IUserService
    {
        private FantasyCupContext _context;

        public UserService(FantasyCupContext context)
        {
            _context = context;
        }

        public User Authenticate(string emaladdress, string password)
        {
            if (string.IsNullOrEmpty(emaladdress) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.User.SingleOrDefault(x => x.EmailAddress == emaladdress);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public User Create(User user, string password, string refreshToken)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new FantasyException("Password is required");

            if (_context.User.Any(x => x.EmailAddress == user.EmailAddress))
                throw new FantasyException("This email address is already registered");

            if (_context.User.Any(x => x.UserName == user.UserName))
                throw new FantasyException("Username " + user.UserName + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = refreshToken;

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public void SaveRefreshToken(int userId, string token)
        {
            var user = _context.User.SingleOrDefault(x => x.Id == userId);

            // check if username exists
            if (user == null)
                throw new FantasyException("User doesn't exist");

            user.RefreshToken = token;
            _context.User.Update(user);
            _context.SaveChanges();
        }

        public bool VerifyRefreshToken(int userId, string token)
        {
            return _context.User.Any(x => x.Id == userId && "Bearer " + x.RefreshToken == token);
        }
    }
}
