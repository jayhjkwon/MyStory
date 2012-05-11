using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using MyStory.Infrastructure.AutoMapper;
using MyStory.Services;

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
            Bind<ITagService>().To<TagService>();
            Bind<IAuthenticationService>().To<FormsAuthenticationService>();
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