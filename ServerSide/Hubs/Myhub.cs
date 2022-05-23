using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSide.Hubs
{
    public class Myhub : Hub
    {
        private readonly IDictionary<string, string> _connections;

        public Myhub(IDictionary<string, string> connections)
        {
            _connections = connections;
        }
        public async Task Changed(string message,string sender, string receiver)
        {
            string x = Context.ConnectionId;
            if(_connections.ContainsKey(receiver))
            {
                await Clients.Client(_connections[receiver]).SendAsync("ChangeRecieved", message, sender);
                //await Clients.AllExcept(Context.ConnectionId).SendAsync("ChangeRecieved", message, sender);
            }
        }

        public async Task ChangedContact(string contactId, string receiver)
        {
            string x = Context.ConnectionId;
            if (_connections.ContainsKey(receiver))
            {
                await Clients.Client(_connections[receiver]).SendAsync("addContact", contactId);
                //await Clients.AllExcept(Context.ConnectionId).SendAsync("ChangeRecieved", message, sender);
            }
        }

        public void MakeConnection(string UserId)
        {
            _connections[UserId] = Context.ConnectionId;
        }
    }
}
