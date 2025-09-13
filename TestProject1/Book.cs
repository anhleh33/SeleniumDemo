using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class Book
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string Availability { get; set; }

        public bool IsDifferentPrice(string currentPrice)
        {
            return Price != currentPrice;
        }

        public bool isTheSameTitle(string title)
        {
            return Title == title;
        }

        public void printBook()
        {
            TestContext.WriteLine($"\"{Title}\",\"{Price}\",\"{Availability}\"\n");
        }
    }
}
