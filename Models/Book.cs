using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Models
{
    public class Book
    { 
        public int Id { get; set; } // Primary Key
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; }
    }
}
