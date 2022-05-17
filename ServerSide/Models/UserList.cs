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
            this.Add(new User { Username = "avi", Password = "123456", DisplayName = "aviiii", Image = "profile2.png", Chats = new List<Chat>() });
            this.Add(new User { Username = "aaaaa", Password = "123456", DisplayName = "aaaaa", Image = "profile2.png", Chats = new List<Chat>() });
            this.MessageId = 0;
            this.ChatId = 0;



            User avi = new User { Username = "avi711", Password = "12345678a", DisplayName = "aviiii", Image = "profile2.png", Chats = new List<Chat>() };
            Chat chat1 = new Chat { id = 0, displayname = "or", server = "local2", name = "or", messages = new List<Message>() };
            chat1.messages.Add(new Message { id = 1, created = DateTime.Now, sent = true, content = "hi or" });
            chat1.messages.Add(new Message { id = 2, created = DateTime.Now, sent = false, content = "hi Avi" });
            chat1.messages.Add(new Message { id = 3, created = DateTime.Now, sent = true, content = "how are you or?" });
            chat1.messages.Add(new Message { id = 4, created = DateTime.Now, sent = false, content = "I am fine thank you" });
            avi.Chats.Add(chat1);


            Chat chat2 = new Chat { id = 1, displayname = "gal", server = "local2", name = "galll", messages = new List<Message>() };
            chat2.messages.Add(new Message { id = 5, created = DateTime.Now, sent = true, content = "hi gal" });
            chat2.messages.Add(new Message { id = 6, created = DateTime.Now, sent = false, content = "hi Avi" });
            chat2.messages.Add(new Message { id = 7, created = DateTime.Now, sent = true, content = "how are you gal?" });
            chat2.messages.Add(new Message { id = 8, created = DateTime.Now, sent = false, content = "I am fine thank you :)" });
            avi.Chats.Add(chat2);

            this.Add(avi);
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
