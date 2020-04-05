using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    class DashboardViewModel : INotifyPropertyChanged
    {
        public IMainModel model;
        public DashboardViewModel(IMainModel model)
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

        //properties
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

    }
}
