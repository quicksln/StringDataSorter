using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringDataSorter.Core.Helpers
{
    /// <summary>
    /// The DataSampler class provides a static method to retrieve a predefined array of sample string data.
    /// </summary>
    public static class DataSampler
    {
        public static string[] Get()
        {
            var data = new string[] { "Ford", "Toyota", "Honda", "Chevrolet", "Nissan", "BMW", "Mercedes", "Audi", "Hyundai", "Kia",
                          "Green Ford", "Yellow Toyota", "Red Honda", "Red Chevrolet", "Yellow Nissan", "Green BMW",
                          "Red Mercedes", "Red Audi", "Green Hyundai", "Yellow Kia",
                          "Ford is the best", "Toyota is the best", "Honda is the best", "Chevrolet is the best", "Nissan is the best",
                          "BMW is the best", "Mercedes is the best", "Audi is the best", "Hyundai is the best", "Kia is the best",
            };
            return data;
        }
    }
}
