using System;
using System.Collections.Generic;
namespace JustToEat.Model.RestarauntModel
{
    public class Restaurants
    {
        public string Name { get; set; }
        public double RatingAverage { get; set; }
        public List<CuisineTypes> CuisineTypes { get; set; }
    }
}
