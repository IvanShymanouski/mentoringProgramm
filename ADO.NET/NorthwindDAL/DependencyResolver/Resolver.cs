using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Ninject;

namespace DependencyResolver
{
    public static class Resolver
    {
        static Resolver()
        {
            Kernel = new StandardKernel();
            Kernel.ConfigurateResolver();
        }

        public static IKernel Kernel { get; private set; }

        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<IOrderRepository>().To<OrderRepository>().InSingletonScope()
                                           .WithConstructorArgument("connectionString", @"Data Source=BEHAPPYPC\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True")
                                           .WithConstructorArgument("provider", "System.Data.SqlClient");

            kernel.Bind<ICustomerRepository>().To<CustomerRepository>().InSingletonScope()
                                          .WithConstructorArgument("connectionString", @"Data Source=BEHAPPYPC\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True")
                                          .WithConstructorArgument("provider", "System.Data.SqlClient");
        }
    }
}