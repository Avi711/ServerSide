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
    }
}
