using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductImg> Images { get; set; }
        public Category Category { get; set; }
    }
}
