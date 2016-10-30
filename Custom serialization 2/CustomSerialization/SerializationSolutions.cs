using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomSerialization.DB;
using CustomSerialization.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CustomSerialization.Surrogates;

namespace CustomSerialization
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(
                new NetDataContractSerializer(
                    new StreamingContext(StreamingContextStates.All, dbContext)), true);

            var categories = dbContext.Categories.ToList();

            tester.SerializeAndDeserialize(categories);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(
                new NetDataContractSerializer(
                    new StreamingContext(StreamingContextStates.All, dbContext)), true);

            var products = dbContext.Products.ToList();

            tester.SerializeAndDeserialize(products);
        }


        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Order_Detail),
                new StreamingContext(StreamingContextStates.All, dbContext),
                new Order_DetailSurrogate());

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(
                new NetDataContractSerializer()
                {
                    SurrogateSelector = selector,
                    Context = new StreamingContext(StreamingContextStates.All, dbContext)
                },
                true);

            var orderDetails = dbContext.Order_Details.ToList();

            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(
                new DataContractSerializer(
                    typeof(IEnumerable<Order>),
                    new DataContractSerializerSettings()
                    {
                        DataContractSurrogate = new OrderSurrogate()
                    }),
                true);
            var orders = dbContext.Orders.ToList();

            tester.SerializeAndDeserialize(orders);
        }
    }
}
