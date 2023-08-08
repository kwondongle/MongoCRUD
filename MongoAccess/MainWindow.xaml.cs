using Mongo.Access.ViewModel;
using Mongo.CRUD.Models;
using System.Windows;

namespace Mongo.Access
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Serilog.ILogger logger)
        {
            InitializeComponent();

            DataContext = new HttpClientSample(logger);
        }
    }
}
