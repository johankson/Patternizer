using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;

namespace Sampleapp.iOS
{
    public static class Bootstrapper
	{
		public static void Initialize()
		{
			//var builder = new Autofac.ContainerBuilder();

			//// Register common stuff
			//Sampleapp.Bootstrapper.Initialize(builder);

			//// Register device specific stuff


			//// Set the resolver
			//var container = builder.Build();
			//var csl = new AutofacServiceLocator(container);
			//ServiceLocator.SetLocatorProvider(() => csl);
		}
	}
}
