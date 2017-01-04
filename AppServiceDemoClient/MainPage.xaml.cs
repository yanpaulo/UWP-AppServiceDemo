using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppServiceDemoClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;
        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private AppServiceConnection _appService;

        private async void timer_Tick(object sender, object e)
        {
            // Add the connection.
            if (this._appService == null)
            {
                var appService = new AppServiceConnection();

                // Here, we use the app service name defined in the app service provider's Package.appxmanifest file in the <Extension> section.
                appService.AppServiceName = "com.yanscorp.appservicedemo";

                // Use Windows.ApplicationModel.Package.Current.Id.FamilyName within the app service provider to get this value.
                appService.PackageFamilyName = "64c4dab4-f0e8-4cae-85f0-7b6717a1bad6_1eh6qe4dz9fxp";

                var status = await appService.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {
                    textBlock.Text = "Failed to connect";
                    return;
                }

                //Only set class attribute if the connection was succesfull.
                this._appService = appService;
            }

            // Call the service.
            var message = new ValueSet();
            message.Add("Request", "GetCallCount");
            AppServiceResponse response = await this._appService.SendMessageAsync(message);
            string result = "";

            if (response.Status == AppServiceResponseStatus.Success)
            {
                int? count = response.Message["Response"] as int?;
                // Get the data  that the service sent  to us.
                if (count.HasValue)
                {
                    textBlock.Text = $"{count ?? 9000}";
                }
            }
        }
    }
}
