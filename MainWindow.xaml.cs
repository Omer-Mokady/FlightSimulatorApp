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
        ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel(new MainModel(new MyTelnetClient()));
            DataContext = vm;
            vm.model.connect("127.0.0.1", 5402);
            vm.model.start();


        
            
        }

        private void headingStatus_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void throttleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //vm.model.UpdateThrottle();
            vm.VMUpadteThrottle(e.NewValue);
        }

        private void aileronSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //vm.model.UpdateAileron(e.NewValue);
            vm.VMUpadteAileron(e.NewValue);
        }
    }
}
