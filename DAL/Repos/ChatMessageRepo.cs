using DAL.EF.Tables;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    internal class ChatMessageRepo : Repo, IChatMessageRepo
    {

        public bool Create(ChatMessage c)
        {
            db.ChatMessages.Add(c);
            return db.SaveChanges() > 0;
        }

        public List<ChatMessage> Get()
        {
            return db.ChatMessages.ToList();
        }

        public ChatMessage Get(int id)
        {
            return db.ChatMessages.Find(id);
        }

        public bool Update(ChatMessage c)
        {
            var exMsg = Get(c.Id);
            db.Entry(exMsg).CurrentValues.SetValues(c);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var data = Get(id);
            db.ChatMessages.Remove(data);
            return db.SaveChanges() > 0;
        }

        public List<ChatMessage> GetByUser(int userId, int take)
        {
            return db.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(take)
                .OrderBy(m => m.CreatedAt)
                .ToList();
        }
    }
}