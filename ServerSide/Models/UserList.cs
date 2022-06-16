using System;
using System.Collections.Generic;

namespace ServerSide.Models
{
    public class UserList
    {
        private static UserList instance;
        private List<User> users = new List<User>();
        public int MessageId { get; set; }
        public int ChatId { get; set; }

        private UserList()
        {
            User shirin = new User { Username = "shirin77", Password = "12345678a", DisplayName = "shirin", Image = "profile2.png", Chats = new List<Chat>() };
            User or = new User { Username = "or77", Password = "12345678a", DisplayName = "or", Image = "profile2.png", Chats = new List<Chat>() };
            User dan = new User { Username = "dan77", Password = "12345678a", DisplayName = "dan", Image = "profile2.png", Chats = new List<Chat>() };
            User kehat = new User { Username = "kehat77", Password = "12345678a", DisplayName = "kehat", Image = "profile2.png", Chats = new List<Chat>() };
            User avi = new User { Username = "avi77", Password = "12345678a", DisplayName = "avi", Image = "profile2.png", Chats = new List<Chat>() };
            User rodin = new User { Username = "rodin77", Password = "12345678a", DisplayName = "rodin", Image = "profile2.png", Chats = new List<Chat>() };
            this.MessageId = 13;
            this.ChatId = 3;



            Chat chat1 = new Chat { id = 0, displayname = "or", server = "local2", name = "or77", messages = new List<Message>() };
            chat1.messages.Add(new Message { id = 1, created = DateTime.Today.AddDays(-5), sent = true, content = "hi or" });
            chat1.messages.Add(new Message { id = 2, created = DateTime.Today.AddDays(-5), sent = false, content = "hi Avi" });
            chat1.messages.Add(new Message { id = 3, created = DateTime.Today.AddDays(-5), sent = true, content = "how are you or?" });
            chat1.messages.Add(new Message { id = 4, created = DateTime.Today.AddDays(-5), sent = false, content = "I am fine thank you" });
            

            Chat chat2 = new Chat { id = 1, displayname = "shirin", server = "local2", name = "shirin77", messages = new List<Message>() };
            chat2.messages.Add(new Message { id = 5, created = DateTime.Today.AddDays(-5), sent = true, content = "hi shirin" });
            chat2.messages.Add(new Message { id = 6, created = DateTime.Today.AddDays(-5), sent = false, content = "hi Avi" });
            chat2.messages.Add(new Message { id = 7, created = DateTime.Today.AddDays(-5), sent = true, content = "how are you shirin?" });
            chat2.messages.Add(new Message { id = 8, created = DateTime.Today.AddDays(-5), sent = false, content = "I am fine thank you :)" });
            
            


            Chat chat3 = new Chat { id = 2, displayname = "avi", server = "local2", name = "avi77", messages = new List<Message>() };
            chat3.messages.Add(new Message { id = 9, created = DateTime.Today.AddDays(-5), sent = false, content = "hi shirin" });
            chat3.messages.Add(new Message { id = 10, created = DateTime.Today.AddDays(-5), sent = true, content = "hi Avi" });
            chat3.messages.Add(new Message { id = 11, created = DateTime.Today.AddDays(-5), sent = false, content = "how are you shirin?" });
            chat3.messages.Add(new Message { id = 12, created = DateTime.Today.AddDays(-5), sent = true, content = "I am fine thank you :)" });


            Chat chat4 = new Chat { id = 3, displayname = "avi", server = "local2", name = "avi77", messages = new List<Message>() };
            chat4.messages.Add(new Message { id = 13, created = DateTime.Today.AddDays(-5), sent = false, content = "hi or" });
            chat4.messages.Add(new Message { id = 14, created = DateTime.Today.AddDays(-5), sent = true, content = "hi Avi" });
            chat4.messages.Add(new Message { id = 15, created = DateTime.Today.AddDays(-5), sent = false, content = "how are you or?" });
            chat4.messages.Add(new Message { id = 16, created = DateTime.Today.AddDays(-5), sent = true, content = "I am fine thank you" });


            avi.Chats.Add(chat1);
            avi.Chats.Add(chat2);
            shirin.Chats.Add(chat3);
            or.Chats.Add(chat4);


            this.Add(shirin);
            this.Add(or);
            this.Add(dan);
            this.Add(avi);
            this.Add(kehat);
            this.Add(rodin);
        }

        
        public static UserList GetInstance()
        {
            if (instance == null)
                instance = new UserList();
            return instance;
        }

        public List<User> Items
        {
            get { return users; }
        }

        public void Add(User item)
        {
            users.Add(item);
        }
    }
}
