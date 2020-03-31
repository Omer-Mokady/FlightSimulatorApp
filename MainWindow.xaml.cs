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
        //
        private JoystickViewModel joystickVm;

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
            myMapObject.DataContext = mapVm;
            joystickObject.DataContext = joystickVm;


            vm.model.connect("127.0.0.1", 5402);
            vm.model.start();


        
            
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
    }
}
