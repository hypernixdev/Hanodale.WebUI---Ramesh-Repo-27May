using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Hanodale.WebUI
{
    public class UnityContainerFactory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope", Justification = "Container has the scope of the application.")
        ]
        public IUnityContainer CreateConfiguredContainer()
        {
            var container = new UnityContainer();
            LoadConfigurationOverrides(container);
            return container;
        }

        private static void LoadConfigurationOverrides(IUnityContainer container)
        {
            container.LoadConfiguration("container");
        }
    }
}