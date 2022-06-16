using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Data;
using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Services
{
    public class ContactService : IContactService
    {
        private readonly ServerSideContext _context;

        public ContactService (ServerSideContext context)
        {
            _context = context;
        }

        public List<Contact> GetAllContacts(User curUser)
        {
            List<Contact> contacts = new List<Contact>();

            curUser.Chats.ForEach(c =>
            {
                contacts.Add(buildContact(c));
            });
            return contacts;
        }
        public Contact ContactDetails(User curUser, string id)
        {
            if (id == null)
            {
                return null;
            }
            Chat chat = curUser.Chats.Where(chat => chat.name == id).FirstOrDefault();
            if (chat == null)
                return null;
            return buildContact(chat);
        }

        public Contact CreateContact(User curUser, Contact contact)
        {

            Chat c = curUser.Chats.Where(c => c.name == contact.id).FirstOrDefault();
            if (c != null)
                return null;

            Chat chat = new Chat();
            chat.name = contact.id;
            chat.displayname = contact.name;
            chat.server = contact.server;
            curUser.Chats.Add(chat);
            _context.Chat.Add(chat);
            _context.SaveChanges();
            return contact;
        }
        public int DeleteContact(User curUser, string id)
        {
            if (id == null)
            {
                return -1;
            }
            Chat chat = curUser.Chats.Where(u => u.name == id).FirstOrDefault();
            if (chat == null)
                return -1;
            curUser.Chats.Remove(chat);
            _context.Chat.Remove(chat);
            _context.SaveChanges();
            return 0;
        }
        public List<Message> GetAllMessages(User curUser, string id)
        {
            if (id == null)
            {
                return null;
            }

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();
            if (chat2 == null)
                return null;
            if (chat2.messages == null)
                return (new List<Message>());
            return (chat2.messages);
        }
        public Message CreateMessage(User curUser, string id, Message message)
        {
            if (id == null)
            {
                return null;
            }

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();
            if (chat2 == null)
                return null;
            message.sent = true;
            message.created = DateTime.Now;
            if (chat2.messages == null)
                chat2.messages = new List<Message>();
            chat2.messages.Add(message);
            //_context.Message.Add(message);
            _context.SaveChanges();
            return message;
        }

        public Message ViewSpecificMessage(User curUser, string id, int id2)
        {
            if (id == null)
            {
                return null;
            }
            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();

            if (chat2.messages == null)
                return null;
            return chat2.messages.FirstOrDefault(m => m.id == id2);
        }
        public Message UpdateSpecificMessage(User curUser, string id, int id2, Message message)
        {
            if (id == null)
            {
                return null;
            }
            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();


            if (chat2.messages == null)
                return null;

            Message msg = chat2.messages.Where(m => m.id == id2).FirstOrDefault();
            if (msg == null)
                return null;

            msg.content = message.content;
            _context.Update(msg);
            _context.SaveChanges();
            return msg;
        }
        public Message DeleteSpecificMwssage(User curUser, string id, int id2)
        {
            if (id == null)
            {
                return null;
            }

            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();
            Message msg = chat2.messages.Where(m => m.id == id2).FirstOrDefault();
            chat.messages.Remove(msg);
            _context.Message.Remove(msg);
            _context.SaveChanges();
            return msg;
        }

        public Contact EditContact(User curUser,string id, Contact contact)
        {
            if (id == null)
            {
                return null;
            }
            Chat chat = curUser.Chats.Where(c => c.name == id).FirstOrDefault();
            if (chat == null)
                return null;
            if (contact.name != null)
                chat.displayname = contact.name;
            if (contact.server != null)
                chat.server = contact.server;
            _context.SaveChanges();
            return contact;
        }
             
       

       

        



        private Contact buildContact(Chat chat)
        {
            Chat chat2 = _context.Chat.Where(c => c.id == chat.id).Include(x => x.messages).FirstOrDefault();
            Contact temp = new Contact();
            if (chat2 != null)
            {
                temp.id = chat2.name;
                temp.name = chat2.displayname;
                temp.server = chat2.server;
                if (chat2.messages != null && chat2.messages.Count > 0)
                {
                    temp.last = chat2.messages.LastOrDefault().content;
                    temp.lastdate = chat2.messages.LastOrDefault().created;

                }
            }
            return temp;
        }
    }
}
