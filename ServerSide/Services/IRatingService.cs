using ServerSide.Models;
using System.Collections.Generic;

namespace ServerSide.Services
{
    public interface IRatingService
    {

        public List<Rating> GetAll();

        public Rating Get(int id);

        public void Create(string comment, string name, int rate);

        public void Edit(int id, string comment, int rate);
        public void Delete(int id);


    }
}
