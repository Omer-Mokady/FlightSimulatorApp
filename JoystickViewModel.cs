using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace FlightSimulatorApp
{
    class JoystickViewModel : INotifyPropertyChanged
    {
        public IMainModel model;

        public JoystickViewModel(IMainModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;


        private double rudder;
        public double VM_Rudder
        {
            //get { return rudder; }
            set
            {
                rudder = value;
                //Console.WriteLine("rudder=" + rudder);
                model.UpdateRudder(rudder);
            }
        }

        private double elevator;
        public double VM_Elevator
        {
            //get { return elevator; }
            set
            {
                elevator = value;
                //Console.WriteLine("elevator=" + elevator);
                model.UpdateElevator(elevator);
            }
        }



    }


}
