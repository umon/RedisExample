using System;
using System.Linq;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext { get; }
        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("products")]
        public IActionResult Products(int page = 1, int size = 3)
        {
            if (page < 1) page = 1;
            if (size < 3) size = 3;
            if (size > 10) size = 10;

            var productQuery = dbContext.Products
                .Where(x => x.InSales);

            var totalProductCount = productQuery.Count();

            var products = productQuery
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var pagedProductList = new PagedProductList
            {
                Products = products,
                TotalPage = (int)Math.Ceiling((double)totalProductCount / size),
                CurrentPage = page
            };

            return View(pagedProductList);
        }
    }
}