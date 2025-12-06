using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IChatMessageRepo : IRepo<ChatMessage, int, bool>
    {
        List<ChatMessage> GetByUser(int userId, int take);
    }
}
