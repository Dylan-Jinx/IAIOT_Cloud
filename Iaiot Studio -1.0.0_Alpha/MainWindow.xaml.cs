using System.Net.Http;
using System.Windows;
using Panuon.UI.Silver;

namespace Iaiot_Studio__1._0._0_Alpha
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
            string url = @"http://localhost:5000/api/v1/Account/Login";
            Auth auth = new Auth()
            {
                telephone = "13850526746",
                password = "123456"
            };
            string test = Common.HttpRequest.PostAsyncJson(url, Common.JsonHelper.Serialize(auth)).Result;
            MessageBox.Show(test);
        }
    }

    public class Auth
    {
        public string telephone { get; set; }
        public string password { get; set; }
    }
}
