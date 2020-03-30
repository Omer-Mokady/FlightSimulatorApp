using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    class MapViewModel : INotifyPropertyChanged
    {
        public IMainModel model;
        public MapViewModel(IMainModel model)
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
        private double latitude;
        public double VM_Latitude
        {
            get
            {
                
                return model.Latitude;
            }
            set
            {
                latitude = value;
            }
        }

        private double longitude;
        public double VM_Longtitude
        {
            get
            {
                return model.Longtitude;
            }
            set
            {
                longitude = value;
            }
        }

        private Location location;
        public Location VM_Location
        {
            get
            {
                location = model.Location;
                return location;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
