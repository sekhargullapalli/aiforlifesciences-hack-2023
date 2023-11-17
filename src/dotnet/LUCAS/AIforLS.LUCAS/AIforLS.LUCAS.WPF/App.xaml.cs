using Microsoft.Extensions.Configuration;

using System.Configuration;
using System.Data;
using System.Windows;

namespace AIforLS.LUCAS.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<App>().Build();                

            //Users need to set the ArcGISRuntimeApiKey in the user secrets manager or as environment variable
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = configuration["ArcGISRuntimeApiKey"]??"";
        }
    }
}