using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerSide.Services
{
    public class RatingService : IRatingService
    {
        private static List<Rating> ratings = new List<Rating>();


        public List<Rating> GetAll()
        {
            return ratings;
        }

        public Rating Get(int id)
        {
            return ratings.Find(x => x.Id == id);
        }

        public void Create(string comment, string name, int rate)
        {
            int nextId = 0;
            if (ratings.Count > 0)
            {
                nextId = ratings.Max(x => x.Id) + 1;
            }

            Rating rating = new Rating();
            rating.Id = nextId;
            rating.Comment = comment;
            rating.Name = name;
            rating.rate = rate;
            rating.PublishedDate = DateTime.Now.ToString();
            ratings.Add(rating);
        }

        public void Edit(int id, string name, string comment, int rate)
        {
            Rating rating = Get(id);
            rating.Name = name;
            rating.Comment = comment;
            rating.rate = rate;
            rating.PublishedDate = DateTime.Now.ToString();

        }
        public void Delete(int id)
        {
            ratings.Remove(Get(id));
        }
    }
}
