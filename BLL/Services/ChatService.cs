using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.External;

namespace BLL.Services
{
    public class ChatService
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChatMessage, ChatMessageDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static async Task<ChatMessageDTO> SendMessage(int userId, string message)
        {
            // 1) save user message
            var userMsg = new ChatMessage
            {
                UserId = userId,
                Role = "user",
                Content = message,
                CreatedAt = DateTime.Now
            };
            DataAccessFactory.ChatMessageData().Create(userMsg);

            // 2) ask Ollama
            var replyText = await OllamaClient.GenerateAsync(message);

            // 3) save bot reply
            var botMsg = new ChatMessage
            {
                UserId = userId,
                Role = "assistant",
                Content = replyText,
                CreatedAt = DateTime.Now
            };
            DataAccessFactory.ChatMessageData().Create(botMsg);

            return GetMapper().Map<ChatMessageDTO>(botMsg);
        }

        public static List<ChatMessageDTO> GetHistory(int userId, int take = 30)
        {
            var data = DataAccessFactory.ChatMessageData().GetByUser(userId, take);
            return GetMapper().Map<List<ChatMessageDTO>>(data);
        }
    }
}
