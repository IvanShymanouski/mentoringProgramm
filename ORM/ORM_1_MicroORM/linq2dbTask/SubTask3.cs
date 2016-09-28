using linq2dbTask.Entities;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace linq2dbTask
{
    [TestClass]
    public class SubTask3
    {
        [TestMethod]
        public void AddNewEmployeeWithTerritoriesListHeIsResponsibleFor()
        {
            using (var connection = new Northwind())
            {
                var id = (int)(decimal)connection.InsertWithIdentity(new Employee() { FirstName = "Yaser", LastName = "Arafat" });

                var i = 0;
                const int terCount = 2;
                var ters = connection.Territories.Select(x => x.Id).Take(terCount).ToArray();
                for(i=0; i<terCount; i++)
                {
                    connection.Insert(new EmployeeTerritory() { EmployeeId = id, TerritoryId = ters[i] });
                }
            }
        }

        [TestMethod]
        public void MoveProductsToTheOtherCategory()
        {
            using (var connection = new Northwind())
            {
                var cat = connection.Categories.First(c => c.Name == "Seafood");
                var prod = connection.Products.First(p => p.CategoryId != cat.Id);
                prod.CategoryId = cat.Id;

                connection.Update(prod);
            }
        }

        [TestMethod]
        public void ScopeProductsAdding()
        {
            using (var connection = new Northwind())
            {
                var products = new List<Product>()
                                   {
                                       new Product()
                                           {
                                               Name = "Product1",
                                               Category = new Category { Name = "Category1" },
                                               Supplier = new Supplier { CompanyName = "Supplier1" }
                                           },
                                       new Product()
                                           {
                                               Name = "Product2",
                                               Category = new Category { Name = "Category2" },
                                               Supplier = new Supplier { CompanyName = "Supplier2" }
                                           },
                                       new Product()
                                           {
                                               Name = "Product3",
                                               Category = new Category { Name = "Category1" },
                                               Supplier = new Supplier { CompanyName = "Supplier2" }
                                           }
                                   };

                foreach (var p in products)
                {
                    if (connection.Categories.Any(c => c.Name.Equals(p.Category.Name)))
                    {
                        p.CategoryId = connection.Categories.First(c => c.Name.Equals(p.Category.Name)).Id;
                    }
                    else
                    {
                        p.CategoryId = (int)(decimal)connection.InsertWithIdentity(new Category() { Name = p.Category.Name });
                    }

                    if (connection.Suppliers.Any(s => s.CompanyName.Equals(p.Supplier.CompanyName)))
                    {
                        p.SupplierId = connection.Suppliers.First(s => s.CompanyName.Equals(p.Supplier.CompanyName)).Id;
                    }
                    else
                    {
                        p.SupplierId = (int)(decimal)connection.InsertWithIdentity(new Supplier() { CompanyName = p.Supplier.CompanyName });
                    }

                    connection.Insert(p);
                }
            }
        }

        [TestMethod]
        public void ReplaceProducts()
        {
            using (var connection = new Northwind())
            {
                var incompleteOrderIds = connection.Orders.Where(o => o.ShippedDate == null).Select(o => o.Id).ToList();
                var incompleteOrders = connection.OrderDetails.ToList().Where(d => incompleteOrderIds.Any(o => o == d.OrderId)).OrderByDescending(io => io.ProductId);
                foreach (var io in incompleteOrders)
                {
                    var newProduct = io.ProductId + 1;
                    if (!connection.Products.Any(p => p.Id == newProduct))
                    {
                        newProduct = connection.Products.First().Id;
                    }

                    var product = connection.Products.First(p => p.Id == newProduct);
                    var newPrice = product.UnitPrice;
                    connection.OrderDetails.Where(od => od.OrderId == io.OrderId && od.ProductId == io.ProductId)
                        .Update(od => new OrderDetails()
                        {
                            ProductId = newProduct,
                            UnitPrice = newPrice
                        });
                }
            }
        }
    }
}
