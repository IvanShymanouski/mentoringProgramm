using DAL.Entities;
using DAL.Interfaces;
using DependencyResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Linq;

namespace DAL.Repositories.Tests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private static ICustomerRepository repo;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            repo = Resolver.Kernel.Get<ICustomerRepository>();
        }

        [TestMethod]
        public void GetIDsTest()
        {
            var customersIDs = repo.GetIDs();

            Assert.IsNotNull(customersIDs, "Value can not be null");
            foreach (var id in customersIDs)
            {
                Console.WriteLine(id);
            }
        }

        [TestMethod]
        public void GetProductQuantitiesTest()
        {
            string firstCustomer = repo.GetIDs().FirstOrDefault();

            Assert.IsNotNull(firstCustomer, "Customer table doesn't have any records");
            Console.WriteLine(firstCustomer);
            Console.WriteLine("------------");

            var productQuantities = repo.GetProductQuantities(firstCustomer);

            Assert.IsNotNull(productQuantities, "ProductQuantities mustn't be null");

            foreach (var pQ in productQuantities)
            {
                Console.WriteLine(pQ.ProductName + " - " + pQ.Quantity);
            }
        }
    }
}