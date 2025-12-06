namespace DAL.Migrations
{
    using DAL.EF.Tables;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.EF.UMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.EF.UMSContext context)
        {
            // Run only when there are no users yet
            if (!context.Users.Any())
            {
                // ----- Users -----
                var u1 = new User
                {
                    UserName = "testuser1",
                    Password = "1234",
                    Name = "Test User One"
                };

                var u2 = new User
                {
                    UserName = "testuser2",
                    Password = "1234",
                    Name = "Test User Two"
                };

                context.Users.Add(u1);
                context.Users.Add(u2);
                context.SaveChanges(); // so Id is generated

                // Reload with Ids (or use u1.Id / u2.Id after SaveChanges)
                var user1 = context.Users.FirstOrDefault(u => u.UserName == "testuser1");
                var user2 = context.Users.FirstOrDefault(u => u.UserName == "testuser2");

                // ----- Chat messages for user1 -----
                if (user1 != null)
                {
                    context.ChatMessages.Add(new ChatMessage
                    {
                        UserId = user1.Id,
                        Role = "user",
                        Content = "Hello, this is my first message.",
                        CreatedAt = DateTime.Now.AddMinutes(-5)
                    });

                    context.ChatMessages.Add(new ChatMessage
                    {
                        UserId = user1.Id,
                        Role = "assistant",
                        Content = "Hi testuser1, I am a fake bot reply.",
                        CreatedAt = DateTime.Now.AddMinutes(-4)
                    });
                }

                // ----- Chat messages for user2 -----
                if (user2 != null)
                {
                    context.ChatMessages.Add(new ChatMessage
                    {
                        UserId = user2.Id,
                        Role = "user",
                        Content = "Yo, anyone here?",
                        CreatedAt = DateTime.Now.AddMinutes(-3)
                    });

                    context.ChatMessages.Add(new ChatMessage
                    {
                        UserId = user2.Id,
                        Role = "assistant",
                        Content = "Hello testuser2, I am another fake reply.",
                        CreatedAt = DateTime.Now.AddMinutes(-2)
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
