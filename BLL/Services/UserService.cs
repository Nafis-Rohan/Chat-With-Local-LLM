using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static bool Create(UserDTO u, string password)
        {
            var data = GetMapper().Map<User>(u);
            data.Password = password;
            return DataAccessFactory.UserData().Create(data);
        }

        public static List<UserDTO> Get()
        {
            var data = DataAccessFactory.UserData().Get();
            return GetMapper().Map<List<UserDTO>>(data);
        }

        public static UserDTO Get(int id)
        {
            var data = DataAccessFactory.UserData().Get(id);
            return GetMapper().Map<UserDTO>(data);
        }

        public static bool Update(UserDTO u, string password = null)
        {
            var data = GetMapper().Map<User>(u);
            if (!string.IsNullOrEmpty(password))
            {
                data.Password = password;
            }
            return DataAccessFactory.UserData().Update(data);
        }

        public static bool Delete(int id)
        {
            return DataAccessFactory.UserData().Delete(id);
        }
    }
}
