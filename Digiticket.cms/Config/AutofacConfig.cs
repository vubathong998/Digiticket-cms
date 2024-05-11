using Autofac;
using Autofac.Integration.Mvc;
using Services.Implement;
using Services.Interfaces;
using System.Reflection;
using System.Web.Mvc;


namespace DigiticketCMS.Config
{
    public static class AutofacConfig
    {
        public static void AutofacConfigContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Business services
            builder.RegisterType<AccountServices>().As<IAccountServices>().InstancePerRequest();
            builder.RegisterType<SupplierServices>().As<ISupplierServices>().InstancePerRequest();
            builder.RegisterType<ProductServies>().As<IProductServices>().InstancePerRequest();
            builder.RegisterType<LibServices>().As<ILibServices>().InstancePerRequest();
            builder.RegisterType<PlaceServices>().As<IPlaceServices>().InstancePerRequest();
            builder.RegisterType<TagsServices>().As<ITagsServices>().InstancePerRequest();
            builder.RegisterType<MediaServices>().As<IMediaServices>().InstancePerRequest();
            builder.RegisterType<ServicePriceServices>().As<IServicepriceServices>().InstancePerRequest();
            builder.RegisterType<GroupServiceService>().As<IGroupServiceService>().InstancePerRequest();
            builder.RegisterType<BannerServices>().As<IBannerServices>().InstancePerRequest();
            builder.RegisterType<ContentServices>().As<IContentServices>().InstancePerRequest();
            builder.RegisterType<CategoryTagService>().As<ICategoryTagService>().InstancePerRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}