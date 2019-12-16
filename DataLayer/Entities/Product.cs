using System;

namespace DataLayer
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InSales { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
