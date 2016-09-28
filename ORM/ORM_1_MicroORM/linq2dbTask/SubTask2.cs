using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace linq2dbTask
{
    [TestClass]
    public class SubTask2
    {
        [TestMethod]
        public void ListOfTheProductsWithCategoryAndSuppliers()
        {
            using (var connection = new Northwind())
            {
                foreach (var p in connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
                {
                    Console.WriteLine("Name: {0}", p.Name);
                    Console.WriteLine("-- Category: {0}", p.Category.Name);
                    Console.WriteLine("-- Supplier: {0}", p.Supplier.CompanyName);
                }
            }
        }

        [TestMethod]
        public void ListOfEmployersWithRegionTheyResponsvleFor()
        {
            using (var connection = new Northwind())
            {
                foreach (var et in connection.EmployeeTerritories
                    .Select(x => new
                    {
                        Name = x.Employee.FirstName + " " + x.Employee.LastName,
                        Region = x.Territory.Region.Description
                    })
                    .Distinct())
                {
                    Console.WriteLine("Name: {0}, Region: {1}", et.Name, et.Region);
                }
            }
        }

        [TestMethod]
        public void NumberOfTheEmployersByRegions()
        {
            using (var connection = new Northwind())
            {
                foreach (var et in connection.EmployeeTerritories
                    .Select(x => new
                    {
                        Employee = x.EmployeeId,
                        Region = x.Territory.Region.Description
                    })
                    .Distinct()
                    .GroupBy(x => x.Region))
                {
                    Console.WriteLine("Region: {0}, Employee count: {1}", et.Key, et.Count());
                }
            }
        }

        [TestMethod]
        public void ListOfEmployersWithShippersTheyHaveBeenWorkingWith()
        {
            using (var connection = new Northwind())
            {
                foreach (var o in connection.Orders
                    .Select(o => new
                    {
                        Employee = o.Employee.FirstName + " " + o.Employee.LastName,
                        Shipper = o.Shipper.Name
                    })
                    .Distinct()
                    .GroupBy(o => o.Employee))
                {
                    Console.WriteLine("Employee: {0}", o.Key);
                    Console.WriteLine("Shippers:");
                    foreach (var empls in o)
                    {
                        Console.WriteLine("-- {0}", empls.Shipper);
                    }
                }
            }
        }
    }
}
