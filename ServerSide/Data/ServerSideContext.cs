using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models;
using ServerSide.Services;

namespace ServerSide.Data
{
    public class ServerSideContext : DbContext
    {
        public ServerSideContext (DbContextOptions<ServerSideContext> options)
            : base(options)
        {
        }

        public DbSet<ServerSide.Models.Rating> Rating { get; set; }

        public static implicit operator ServerSideContext(RatingService v)
        {
            throw new NotImplementedException();
        }

        public DbSet<ServerSide.Models.User> User { get; set; }

        public DbSet<ServerSide.Models.Contact> Contact { get; set; }

        public DbSet<ServerSide.Models.Message> Message { get; set; }

        public DbSet<ServerSide.Models.Chat> Chat { get; set; }
    }
}
