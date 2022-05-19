using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ServerSide.Hubs
{
    public class Myhub : Hub
    {
        public async Task Changed(string value)
        {
            await Clients.All.SendAsync("ChangeRecieved", value);

        }
    }
}
