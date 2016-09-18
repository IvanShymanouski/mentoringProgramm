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
    public class OrderRepositoryTests
    {
        private static IOrderRepository repo;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
             repo = Resolver.Kernel.Get<IOrderRepository>();
        }

        [TestMethod]
        public void GetAllTest()
        {
            var orders = repo.GetAll();

            foreach(var order in orders)
            {
                Assert.IsNull(order.Details, "Details should be empty");
            }
        }

        [TestMethod]
        public void GetOrderDetailByOrderIDTest()
        {
            Order firstOrder = repo.GetAll().FirstOrDefault();

            Assert.IsNotNull(firstOrder, "Order table doesn't have any records");

            Order fullOrder = repo.GetOrderDetailByOrderID(firstOrder.OrderID);

            Assert.IsNotNull(fullOrder, "Order mustn't be null");
            Assert.IsNotNull(fullOrder.Details, "Order.Detail mustn't be null");

            Console.WriteLine("Details count is {0}",fullOrder.Details.Count());

            foreach (var detail in fullOrder.Details)
            {
                Assert.IsNotNull(detail.Product, "Order.Detail.Product mustn't be null");
                Assert.AreEqual(detail.Order, fullOrder, "Detail should reference to parent order");
            }
        }

        [TestMethod]
        public void AddTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23 };
            var newId = repo.Add(newOrder);

            Assert.IsTrue(newId > 0, "Object are not created");
        }

        #region Update
        [TestMethod]
        public void UpdateNewOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23 };
            newOrder.OrderID = repo.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repo.Update(newOrder);
            Assert.AreEqual(affected, 1, "Order are not updated");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(newFreight, fullOrder.Freight, "Order are not updated");
        }

        [TestMethod]
        public void UpdateInProgressOrderTest()
        {
            var oldFreight = (decimal)1.23;
            var newOrder = new Order() { OrderID = 1, Freight = oldFreight, OrderDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repo.Update(newOrder);
            Assert.AreEqual(affected, 0, "InProgress order should not be updated");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(oldFreight, fullOrder.Freight, "InProgress order should not be updated");
        }

        [TestMethod]
        public void UpdateDoneOrderTest()
        {
            var oldFreight = (decimal)1.23;
            var newOrder = new Order() { OrderID = 1, Freight = oldFreight, OrderDate = DateTime.Now, ShippedDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var newFreight = newOrder.Freight += 1;
            var affected = repo.Update(newOrder);
            Assert.AreEqual(affected, 0, "Done order should not be updated");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(oldFreight, fullOrder.Freight, "Done order should not be updated");
        }
        #endregion

        #region Delete
        [TestMethod]
        public void DeleteNewOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23 };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not deleted");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNull(fullOrder, "Order are not deleted");
        }

        [TestMethod]
        public void DeleteInProgressOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not deleted");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNull(fullOrder, "Order are not deleted");
        }

        [TestMethod]
        public void DeleteDoneOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now, ShippedDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.Delete(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order should not be deleted");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.IsNotNull(fullOrder, "Order should not be deleted");
        }
        #endregion

        #region SetInProgressStatus
        [TestMethod]
        public void SetInProgressStatusNewOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23 };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not set to InProgress");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.InProgress, "Order are not set to InProgress");
        }

        [TestMethod]
        public void SetInProgressStatusInProgressOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to InProgress");
        }

        [TestMethod]
        public void SetInProgressStatusDoneOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now, ShippedDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetInProgressStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to InProgress");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.Done, "Order are not set to InProgress");
        }
        #endregion

        #region SetDoneStatus
        [TestMethod]
        public void SetDoneStatusNewOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23 };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to Done");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.New, "Order are not set to Done");
        }

        [TestMethod]
        public void SetDoneStatusInProgressOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 1, "Order are not set to Done");

            Order fullOrder = repo.GetOrderDetailByOrderID(newOrder.OrderID);
            Assert.AreEqual(fullOrder.Status, OrderStatus.Done, "Order are not set to Done");
        }

        [TestMethod]
        public void SetDoneStatusDoneOrderTest()
        {
            var newOrder = new Order() { OrderID = 1, Freight = (decimal)1.23, OrderDate = DateTime.Now, ShippedDate = DateTime.Now };
            newOrder.OrderID = repo.Add(newOrder);

            var affected = repo.SetDoneStatus(newOrder.OrderID);
            Assert.AreEqual(affected, 0, "Order are not set to Done");
        }
        #endregion

        [TestMethod]
        public void GetCustOrdersDetailsTest()
        {
            Order firstOrder = repo.GetAll().FirstOrDefault();

            Assert.IsNotNull(firstOrder, "Orders table doesn't have any records");
            Console.WriteLine(firstOrder.OrderID);
            Console.WriteLine("------------");

            var custOrdersDetails = repo.GetCustOrdersDetails(firstOrder.OrderID);

            Assert.IsNotNull(custOrdersDetails, "CustOrdersDetails mustn't be null");

            foreach (var cOD in custOrdersDetails)
            {
                Console.WriteLine(cOD.ProductName);
                Console.WriteLine("-- UnitPrice = " + cOD.UnitPrice);
                Console.WriteLine("-- Quantity = " + cOD.Quantity);
                Console.WriteLine("-- Discount = " + cOD.Discount);
                Console.WriteLine("-- ExtendedPrice = " + cOD.ExtendedPrice);
            }
        }
    }
}