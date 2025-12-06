using DAL.EF.Tables;
using DAL.EF;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    internal class UserRepo : Repo, IRepo<User, int, bool>, IAuth<bool>
    {

        public bool Authenticate(string userName, string password)
        {
            var data = db.Users.FirstOrDefault(u =>u.UserName.Equals(userName) && u.Password.Equals(password));
            return data != null;
        }

        public bool Create(User u)
        {
            db.Users.Add(u);
            return db.SaveChanges() > 0;
        }

        public List<User> Get()
        {
            return db.Users.ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public bool Update(User u)
        {
            var exUser = Get(u.Id);
            db.Entry(exUser).CurrentValues.SetValues(u);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = Get(id);
            db.Users.Remove(data);
            return db.SaveChanges() > 0;
        }

        public User GetByUserName(string userName)
        {
            return db.Users.FirstOrDefault(u => u.UserName.Equals(userName));
        }
    }
}
