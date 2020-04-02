using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private string userIpAddress;
        private int userPortNumber;
        //MainModel model;

        public ConnectionWindow()
        {
            InitializeComponent();
        }


        public string GetIp()
        {
            return this.userIpAddress;
        }

        public int GetPort()
        {
            return this.userPortNumber;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.userIpAddress = UserIpBox.Text;
            this.userPortNumber = int.Parse(UserPortBox.Text);
            this.Close();
        }
    }
}