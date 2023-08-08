using Mongo.Common;
using Serilog;
using System;
using System.Windows;

namespace Mongo.Access
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void OnApplicationStartup(object sender, StartupEventArgs se)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("./logs/log_.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                new MainWindow(Log.Logger).ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
        }
    }
}
