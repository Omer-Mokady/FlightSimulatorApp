using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace FlightSimulatorApp
{
    class ViewModel : INotifyPropertyChanged
    {
        public IMainModel model;
        public ViewModel(IMainModel model)
        {
            this.model = model;

            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        // properties
        public double VM_HeadingDeg
        {
            get { return model.HeadingDeg; }
        }
        public double VM_GpsVerticalSpeed
        {
            get { return model.GpsVerticalSpeed; }
        }

        public double VM_GpsGroundSpeed
        {
            get { return model.GpsGroundSpeed; }
        }

        public double VM_GpsAltitude
        {
            get { return model.GpsAltitude; }
        }

        public double VM_AirSpeed
        {
            get { return model.AirSpeed; }
        }

        public double VM_InternalRollDeg
        {
            get { return model.InternalRollDeg; }
        }

        public double VM_InternalPitchDeg
        {
            get { return model.InternalPitchDeg; }
        }

        public double VM_AltimeterAltitude
        {
            get { return model.AltimeterAltitude; }
        }

        private double aileron;
        private double VM_Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                // model.func?
            }
        }

        private double throttle;
        private double VM_Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                // model.func?
            }
        }


        // to make one VM_Location instead rudder and elevator? eli - 4 - gimel- 10:15

        private double rudder;
        private double VM_Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                // model.func?
            }
        }

        private double elevator;
        private double VM_Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                // model.func?
            }
        }





    }
}
