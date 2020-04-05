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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainModel model;
        private MyTelnetClient client;
        private ViewModel vm;
        private MapViewModel mapVm;
        private JoystickViewModel joystickVm;
        private DashboardViewModel dashboardVM;
        private bool isClicked = false;

        private string ipAddress;
        private int portNumber;


        public MainWindow()
        {
            InitializeComponent();
            client = new MyTelnetClient();
            model = new MainModel(client);
            vm = new ViewModel(model);
            mapVm = new MapViewModel(model);

            //
            joystickVm = new JoystickViewModel(model);
            DataContext = vm;

            dashboardVM = new DashboardViewModel(model);

            myMapObject.DataContext = mapVm;
            joystickObject.DataContext = joystickVm;
            dashboadObject.DataContext = dashboardVM;


            //vm.model.connect("127.0.0.1", 5402);
            //vm.model.start();


        
            
        }

        private void headingStatus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void throttleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //vm.VMUpadteThrottle(e.NewValue);
        }

        private void aileronSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //vm.VMUpadteAileron(e.NewValue);
        }

        private void cleanButtom_Click(object sender, RoutedEventArgs e)
        {
            exceptionsText.Clear();
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(model.GetBoolRunning());
            if (model.GetBoolRunning())
            {
                ConnectionWindow connect = new ConnectionWindow();
                connect.ShowDialog();
                isClicked = true;
                this.ipAddress = connect.GetIp();
                bool access = true;
                if (connect.GetPort() != null)
                {
                    try
                    {
                        this.portNumber = int.Parse(connect.GetPort());
                    }
                    catch
                    {
                        exceptionsText.Text = "error - port can have only digits\ntry again\n";
                        access = false;
                    }
                    if (access)
                    {

                        vm.model.connect(this.ipAddress, this.portNumber);
                        //vm.model.start();             
                    }

                }
            } else
            {
                exceptionsText.Text = "you are already connect\nplease press the disconnect buttom before\n";
            }
                
                
                


            
        }

        private void disconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!model.GetBoolRunning())
            {
                //isClicked = false;
                vm.model.disconnect();
            } else
            {
                exceptionsText.Text = "you are already not connected\n";
            }
        }
    }
}
