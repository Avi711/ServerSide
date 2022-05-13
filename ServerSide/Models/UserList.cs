using System.Collections.Generic;

namespace ServerSide.Models
{
    public class UserList
    {
        private static UserList instance;
        private List<User> users = new List<User>();

        private UserList()
        {
            this.Add(new User { Username = "avi", Password = "123456", DisplayName = "aviiii", Image = "apple", Chats = new List<Chat>() });
            this.Add(new User { Username = "aaaaa", Password = "123456", DisplayName = "aaaaa", Image = "apple", Chats = new List<Chat>() });
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
