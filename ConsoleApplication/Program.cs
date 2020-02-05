using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cache;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleApplication
{
    class Program
    {
        static string commandHelpMessage = "\n# Ürün stok listesi için:\t list-stocks \n# Ürün stok güncellemek için:\t update-stock <barcode> <quatity> \n# Kapatmak için:\t\t exit\n";
        static string commandErrorMessage = "\n!!! Yanlış komut girdiniz. \n" + commandHelpMessage;

        static void Main(string[] args)
        {
            var command = "";
            Console.WriteLine(commandHelpMessage);

            while (!command.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                command = Console.ReadLine();

                if (command.StartsWith("update-stock "))
                {
                    var barcode = "";
                    var quantity = 0;
                    var commandParameters = command.Replace("update-stock ", "").Split(" ");

                    if (commandParameters.Length > 0) barcode = commandParameters[0];
                    if (commandParameters.Length > 1) int.TryParse(commandParameters[1], out quantity);

                    if (!string.IsNullOrEmpty(barcode) && quantity != 0)
                    {
                        UpdateStockQuantity(barcode, quantity);
                        continue;
                    }
                }
                else if (command.Equals("list-stocks"))
                {
                    ListProducts();
                    continue;
                }

                Console.WriteLine(commandErrorMessage);
            }
        }

        static void UpdateStockQuantity(string barcode, int quatity)
        {
            using (var context = new ApplicationDbContext(GetDbContextOptions()))
            {
                var product = context.Products.SingleOrDefault(x => x.Barcode == barcode);

                if (product == null)
                {
                    Console.WriteLine("\n!!! Ürün bulunamadı.\n");
                    return;
                }

                product.Quantity += quatity;
                product.ModifiedDate = DateTime.Now;

                if (context.SaveChanges() > 0)
                {
                    var redisManager = new RedisCacheManager();
                    redisManager.RemoveByPattern("*products*");

                    Console.WriteLine($"\n!!! Stok başarıyla güncellendi: {product.Name}\n");
                }
            }
        }

        static void ListProducts()
        {
            using (var context = new ApplicationDbContext(GetDbContextOptions()))
            {
                var products = context.Products.ToList();

                foreach (var product in products)
                {
                    var stockText =
                    "-------------\n" +
                    $"- Ürün adı: {product.Name}\n" +
                    $"- Barkod: {product.Barcode}\n" +
                    $"- Stok adedi: {product.Quantity} Ürün {(product.InSales ? "satışta" : "satışta değil")}" +
                    $"(son güncellenme: {product.ModifiedDate.ToString("dd.MM.yyyy HH:mm:ss")}\n" +
                    "-------------\n";

                    Console.WriteLine(stockText);
                }
            }
        }
        static DbContextOptions GetDbContextOptions()
        {
            var connectionString = GetConnectionString();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbContextOptionsBuilder.UseSqlServer(connectionString);

            return dbContextOptionsBuilder.Options;
        }

        static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();

            return configuration.GetConnectionString("applicationDbConnectionString");
        }
    }
}
