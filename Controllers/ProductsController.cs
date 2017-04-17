using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProductApp.Models;
using System.Data.Entity;

namespace ProductApp.Controllers
{
    public class ProductsController : ApiController
    {
        private const string CacheKey = "ProductStore";
        private Product[] products;

        public ProductsController()
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    products = new Product[]
                    {
                         new Product { Id = 1, Name = "Bread", Category = "Food", Price = 3.50M },
                         new Product { Id = 2, Name = "Couch", Category = "Furniture", Price = 375.99M },
                         new Product { Id = 3, Name = "Vinyl", Category = "Music", Price = 16.99M }
                    };
                    ctx.Cache[CacheKey] = products;
                }
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
               return (Product[])ctx.Cache[CacheKey];
             
            }

            return new Product[]
            {
                new Product
                {
                    Id = 0,
                    Name = "Test Item",
                    Category = "Test Category",
                    Price = 0
                }
            };
        }

        public IHttpActionResult GetProduct(int id)

        {
            Product product = null;
            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                products = (Product[])ctx.Cache[CacheKey];
               product = products.FirstOrDefault((p) => p.Id == id);
            }
             
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult PostProduct(Product prod)
        {
            IHttpActionResult response = BadRequest();//Request.(System.Net.HttpStatusCode.BadRequest, prod);

            var ctx = HttpContext.Current;
            if (ctx != null)
            {
                try
                {
                    var currentData = ((Product[])ctx.Cache[CacheKey]).ToList();
                    currentData.Add(prod);
                    ctx.Cache[CacheKey] = currentData.ToArray();
                    response = Created(Request.RequestUri,prod);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return response;
        }

    }
}