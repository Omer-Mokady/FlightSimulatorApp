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


namespace FlightSimulatorApp.controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        public Joystick()
        {
            InitializeComponent();
           
        }
        private Point startPoint = new Point();

        private void centerKnob_Completed(object sender, EventArgs e) {
    
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            //if(e.LeftButton == MouseButtonState.Pressed)
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double xValue = e.GetPosition(this).X - startPoint.X;
                double yValue = e.GetPosition(this).Y - startPoint.Y;
                //Console.WriteLine("xvalue = " + xValue + "yvalue = " + yValue);
                //Console.WriteLine("Base.Width / 2 = " + (Base.Width / 2));

                
                if (Math.Sqrt((xValue*xValue) + (yValue*yValue)) < blackCircle.Width / 2)
                {
                    //Console.WriteLine("inside2");

                    knobPosition.X = xValue;
                    knobPosition.Y = yValue;
                    //Console.WriteLine("knobPosition.X = " + knobPosition.X);
                    //Console.WriteLine("knobPosition.Y = " + knobPosition.Y);
                }

            }
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                //Console.WriteLine("mouseDown");

                // initializing start Point
                startPoint = e.GetPosition(this);
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("mouse up");

            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
    }
}
