using System.Collections.Generic;
using DataLayer;

namespace WebApplication.Models
{
    public class PagedProductList
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<Product> Products { get; set; }
    }
}