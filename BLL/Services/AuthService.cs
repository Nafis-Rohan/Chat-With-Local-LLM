using AutoMapper;
using DAL.EF.Tables;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Services
{
    public class AuthService
    {
        public static TokenDTO Authenticate(string userName, string password)
        {
            var res = DataAccessFactory.AuthData().Authenticate(userName, password);
            if (res)
            {
                var token = new Token();
                token.UserName = userName;
                token.CreatedAt = DateTime.Now;
                token.TKey = Guid.NewGuid().ToString();

                var ret = DataAccessFactory.TokenData().Create(token);
                if (ret != null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Token, TokenDTO>().ReverseMap();
                    });
                    var mapper = new Mapper(config);
                    return mapper.Map<TokenDTO>(ret);
                }
            }
            return null;
        }

        public static bool IsTokenValid(string tkey)
        {
            var extk = DataAccessFactory.TokenData().Get(tkey);
            if (extk != null && extk.ExpireAt == null)
            {
                return true;
            }
            return false;
        }

        public static string GetUserNameFromToken(string tkey)
        {
            var extk = DataAccessFactory.TokenData().Get(tkey);
            return extk?.UserName; // safe, returns null if extk is null
        }

        public static bool Logout(string tkey)
        {
            // Find the token by its key
            var extk = DataAccessFactory.TokenData().Get(tkey);
            if (extk == null) return false;          // token not found
            if (extk.ExpireAt != null) return false; // already expired

            // expire it now
            extk.ExpireAt = DateTime.Now;
            return DataAccessFactory.TokenData().Update(extk) != null;
        }
    }
}
