using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using MyStory.Infrastructure.AutoMapper;

namespace MyStory.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            Bind<IMapper>().To<PostMapper>();
            Bind<IMapper>().To<CommentMapper>();
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            return kernel.Bind<T>();
        }

        public IKernel Kernal
        {
            get { return this.kernel; }
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}