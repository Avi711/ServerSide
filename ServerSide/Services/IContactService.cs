using Microsoft.AspNetCore.Mvc;
using ServerSide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSide.Services
{
    public interface IContactService
    {
        public List<Contact> GetAllContacts(User curUser);
        public Contact ContactDetails(User curUser, string id);
        public Contact CreateContact(User curUser, Contact contact);
        public Contact EditContact(User curUser,string id, Contact contact);
        public int DeleteContact(User curUser, string id);
        public List<Message> GetAllMessages(User curUser, string id);
        public Message CreateMessage(User curUser, string id, Message message);
        public Message ViewSpecificMessage(User curUser, string id, int id2);
        public Message UpdateSpecificMessage(User curUser, string id,int id2, Message message);
        public Message DeleteSpecificMwssage(User curUser, string id, int id2);
    }
}
