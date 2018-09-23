using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnGetToken.Click += (s1, e1) => GetTokenAsync();
            btnGetValue.Click += (s1, e1) => GetValueAsync();
        }

        private string webUrl = "http://localhost:5000";

        private string token = "";

        private async Task GetTokenAsync()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    //地址
                    string path = $"{webUrl}/Home/GetToken?userName=user&password=111";

                    token = await client.DownloadStringTaskAsync(path);

                    txbMsg.Text = $"获取到令牌={token}";
                }
            }
            catch (Exception ex)
            {
                txbMsg.Text = $"获取令牌出错={ex.Message}";
            }
        }

        private async Task GetValueAsync()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    //地址
                    string path = $"{webUrl}/api/Value";

                    client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token}");

                    string value = await client.DownloadStringTaskAsync(path);

                    txbMsg.Text = $"获取到数据={value}";
                }
            }
            catch (Exception ex)
            {
                txbMsg.Text = $"获取数据出错={ex.Message}";
            }
        }
    }
}
