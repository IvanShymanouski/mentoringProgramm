// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;
using System.Text.RegularExpressions;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Where - Task 1")]
        [Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
        public void Linq1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 2")]
        [Description("This sample return return all presented in market products")]
        public void Linq2()
        {
            var products =
                from p in dataSource.Products
                where p.UnitsInStock > 0
                select p;

            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }


        [Category("LINQ1 tasks")]
        [Title("Where - Task 1")]
        [Description("Customers with total sum of order where sum greater than X")]
        public void Linq001()
        {
            var rand = new Random();
            var attmeptCount = 2;
            for (var i = 0; i < attmeptCount; i++)
            {
                var x = rand.Next(300, 11000);
                ObjectDumper.Write(x);


                /*
                 * if we talk about DRY :) It's option
                var neededCustomers = new List<Tuple<decimal, string>>();
                var cCount = dataSource.Customers.Count;
                for (var cI = 0; cI < cCount; cI++)
                {
                    var c = dataSource.Customers[cI];
                    var sum = (from o in c.Orders
                               select o.Total
                               ).Sum();
                    if (sum > x)
                    {
                        neededCustomers.Add(new Tuple<decimal, string>(sum, c.CustomerID));
                    }
                }
                */

                var customers =
                    from c in dataSource.Customers
                    where
                    (from o in c.Orders
                     select o.Total
                    ).Sum() > x
                    select new { sum = c.Orders.Select(o => o.Total).Sum(), customer = c.CustomerID };

                foreach (var c in customers)
                {
                    ObjectDumper.Write(c);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 2")]
        [Description("Suppliers for customers with the same country and city")]
        public void Linq002()
        {

            /*
            // without groping
            var customers =
                from c in dataSource.Customers
                select
                    new
                    {
                        c.CustomerID,
                        supliers = from s in dataSource.Suppliers
                                   where s.Country == c.Country && c.City == s.City
                                   select s.SupplierName
                    };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c.CustomerID);
                foreach (var s in c.suppliers)
                {
                    ObjectDumper.Write("-- " + s);
                }
            }
            */

            var customersGroups =
               from c in dataSource.Customers
               group c by new { c.Country, c.City } into CountryCity
               select
                   new
                   {
                       Customers = CountryCity,
                       CountryCity.Key.Country,
                       CountryCity.Key.City,
                       suppliers = from s in dataSource.Suppliers
                                   where s.Country == CountryCity.Key.Country && CountryCity.Key.City == s.City
                                   select s
                   };

            foreach (var cg in customersGroups)
            {
                ObjectDumper.Write(String.Format("Country - {0}, City - {1}", cg.Country, cg.City));
                foreach (var c in cg.Customers)
                {
                    ObjectDumper.Write("- " + c.CustomerID);
                }
                foreach (var s in cg.suppliers)
                {
                    ObjectDumper.Write("--- " + s.SupplierName);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 3")]
        [Description("Customers with orders total greater than X")]
        public void Linq003()
        {
            var rand = new Random();
            var attmeptCount = 2;
            for (var i = 0; i < attmeptCount; i++)
            {
                var x = rand.Next(300, 11000);
                ObjectDumper.Write(x);
                var customers =
                         from c in dataSource.Customers
                         where
                            (from o in c.Orders
                             where o.Total > x
                             select o.Total
                            ).Count() > 0
                         select new { max = c.Orders.Select(o => o.Total).Max(), customer = c.CustomerID };

                foreach (var c in customers)
                {
                    ObjectDumper.Write(c);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 4")]
        [Description("Customers with start date")]
        public void Linq004()
        {
            var customers = dataSource.Customers.Select(c =>
            {
                var firstOrder =
                (from o in c.Orders
                 orderby o.OrderDate.Year, o.OrderDate.Month ascending
                 select o).FirstOrDefault();


                return
                    (firstOrder == null) ?
                    new
                    {
                        c.CustomerID,
                        startMonth = 0,
                        startYear = 0
                    }
                    :
                    new
                    {
                        c.CustomerID,
                        startMonth = firstOrder.OrderDate.Month,
                        startYear = firstOrder.OrderDate.Year
                    };
            });

            foreach (var c in customers)
            {
                if (c.startYear == 0)
                {
                    ObjectDumper.Write(c.CustomerID + " : without any orders");
                }
                else
                {
                    ObjectDumper.Write(c.CustomerID + " : Year - " + c.startYear + "; Month - " + c.startMonth);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 5")]
        [Description("Sorted list of customers with start date")]
        public void Linq005()
        {
            var customers = dataSource.Customers.Select(c =>
            {
                var firstOrder =
                (from o in c.Orders
                 orderby o.OrderDate.Year, o.OrderDate.Month ascending
                 select o).FirstOrDefault();

                int month = 0, year = 0;

                if (firstOrder != null)
                {
                    month = firstOrder.OrderDate.Month;
                    year = firstOrder.OrderDate.Year;
                }

                return
                    new
                    {
                        c.CustomerID,
                        startMonth = month,
                        startYear = year,
                        cashFlow = c.Orders.Select(o => o.Total).Sum()
                    };
            }).OrderByDescending(c => c.startYear)
              .ThenByDescending(c => c.startMonth)
              .ThenByDescending(c => c.cashFlow);

            foreach (var c in customers)
            {
                if (c.startYear == 0)
                {
                    ObjectDumper.Write(c.CustomerID + " : without any orders");
                }
                else
                {
                    ObjectDumper.Write(c.CustomerID + " : Year - " + c.startYear + "; Month - " + c.startMonth + "; Cash flow - " + c.cashFlow);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 6")]
        [Description("Customers with not numeric code or with empty region or with wrong phone number")]
        public void Linq006()
        {
            int number;

            var pattern = @"^\([\S\s]*\)[\S\s]*";

            var customers =
                 from c in dataSource.Customers
                 where c.Region == null &&
                       !Int32.TryParse(c.PostalCode, out number) &&
                       !Regex.IsMatch(c.Phone, pattern)
                 select c;

            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer.CustomerID + " " + customer.Phone);
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 7")]
        [Description("Products which is grouped by category than by inStock than order by price")]
        public void Linq007()
        {
            var productsGroups =
               from p in dataSource.Products
               group p by p.Category into CatProd
               select
                   new
                   {
                       category = CatProd.Key,
                       inStockGroups = from cP in CatProd
                                       group cP by cP.UnitsInStock into StProd
                                       select
                                           new
                                           {
                                               inStock = StProd.Key,
                                               products = StProd.OrderBy(pr => pr.UnitPrice)
                                           }
                   };

            foreach (var pg in productsGroups)
            {
                ObjectDumper.Write("Category - " + pg.category);
                foreach (var p in pg.inStockGroups)
                {
                    ObjectDumper.Write("- InStock - " + p.inStock);
                    foreach (var prod in p.products)
                    {
                        ObjectDumper.Write("-- " + prod.ProductName);
                    }
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 8")]
        [Description("Grouped products in three group (cheap, medium price, expensive)")]
        public void Linq008()
        {
            const int mediumLowLevel = 12, mediumHighLevel = 25;

            var productsGroups = dataSource.Products
                                           .GroupBy(
                                                prod =>
                                                {
                                                    string[] pricesLevel = new string[] { "cheep", "medium price", "expensive" };

                                                    var level = 0;
                                                    if (prod.UnitPrice >= mediumLowLevel)
                                                    {
                                                        if (prod.UnitPrice <= mediumHighLevel)
                                                        {
                                                            level = 1;
                                                        }
                                                        else
                                                        {
                                                            level = 2;
                                                        }
                                                    }

                                                    return pricesLevel[level];
                                                },
                                                prod =>
                                                {
                                                    return new { prod.ProductName, prod.UnitPrice };
                                                });

            ObjectDumper.Write("medium price is beetwen " + mediumLowLevel + " and " + mediumHighLevel);

            foreach (var pg in productsGroups)
            {
                ObjectDumper.Write("Category - " + pg.Key);
                foreach (var cp in pg)
                {
                    ObjectDumper.Write("Name - " + cp.ProductName + "; Price - " + cp.UnitPrice);
                }
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 9")]
        [Description("Average total of orders and average count of orders per city")]
        public void Linq009()
        {
            var cityGroups = from c in dataSource.Customers
                             group c by c.City into cityCustomers
                             select
                                new
                                {
                                    cityCustomers.Key,
                                    AvgSum = cityCustomers.Average(c => c.Orders.Sum(o => o.Total)),
                                    AvgInten = cityCustomers.Average(c => c.Orders.Length)
                                };

            foreach (var cg in cityGroups)
            {
                ObjectDumper.Write("City - " + cg.Key);
                ObjectDumper.Write("- Average Income: " + cg.AvgSum + ";");
                ObjectDumper.Write("- Average Intensity: " + cg.AvgInten);
            }
        }

        [Category("LINQ1 tasks")]
        [Title("Where - Task 10")]
        [Description("Statistics per year and per month and per specific months for each year")]
        public void Linq010()
        {
            var clients = from c in dataSource.Customers
                          select
                            new
                            {
                                Customer = c.CustomerID,
                                Month = from o in c.Orders
                                        group o by o.OrderDate.Month into monthGroup
                                        select 
                                            new 
                                            {
                                                Month = monthGroup.Key,
                                                Stat = monthGroup.Average(o => o.Total) 
                                            },
                                Year = from o in c.Orders
                                       group o by o.OrderDate.Year into yearGroup
                                       select
                                           new 
                                           {
                                               Year = yearGroup.Key,
                                               Stat = yearGroup.Average(o => o.Total) 
                                           },
                                MonthYear = from o in c.Orders
                                            group o by new { o.OrderDate.Year, o.OrderDate.Month } into yearMonthGroup
                                            select
                                                new 
                                                {
                                                    yearMonthGroup.Key.Month,
                                                    yearMonthGroup.Key.Year,
                                                    Stat = yearMonthGroup.Average(o => o.Total) 
                                                }
                            };

            foreach (var c in clients)
            {
                ObjectDumper.Write("ClientId: " + c.Customer);
                ObjectDumper.Write("- Month activity");
                foreach (var m in c.Month)
                {
                    ObjectDumper.Write("-- Month: " + m.Month);
                    ObjectDumper.Write("-- Average Income: " + m.Stat + Environment.NewLine);
                }
                ObjectDumper.Write("- Year activity");
                foreach (var y in c.Year)
                {
                    ObjectDumper.Write("-- Year: " + y.Year);
                    ObjectDumper.Write("-- Average Income: " + y.Stat + Environment.NewLine);
                }
                ObjectDumper.Write("- Month-Year activity");
                foreach (var my in c.MonthYear)
                {
                    ObjectDumper.Write("-- Month: " + my.Month + "; Year: " + my.Year);
                    ObjectDumper.Write("-- Average Income: " + my.Stat + Environment.NewLine);
                }
            }
        }
    }
}
