﻿using System;
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
using System.Configuration;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        private string userIpAddress;
        private string userPortNumber;
        //MainModel model;

        public ConnectionWindow()
        {
            InitializeComponent();
            UserIpBox.Text = ConfigurationManager.AppSettings["DefaultIP"];
            UserPortBox.Text = ConfigurationManager.AppSettings["DefaultPort"];
        }


        public string GetIp()
        {
            return this.userIpAddress;
        }

        public string GetPort()
        {
            return this.userPortNumber;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.userIpAddress = UserIpBox.Text;
            this.userPortNumber = UserPortBox.Text;
            this.Close();
        }
    }
}