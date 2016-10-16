﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Ninject;

namespace MvcMusicStore.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            ConfigurateResolver();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public void ConfigurateResolver()
        {
            _kernel.Bind<ILogger>().To<Logger>();
        }
    }
}