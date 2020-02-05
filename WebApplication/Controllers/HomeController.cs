using System;
using System.Collections.Generic;
using System.Linq;
using Cache;
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

            var redisManager = new RedisCacheManager();
            var pagedProductListKey = $"products|page={page}|size={size}";

            var products = redisManager.Get<List<Product>>(pagedProductListKey);

            if (products == null)
            {
                products = productQuery
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();

                redisManager.Set(pagedProductListKey, products, 60);
            }

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