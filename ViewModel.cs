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
        

        public string VM_StrException
        {
            get { return model.StrException; }
        }

        private double aileron;
        public double VM_Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                model.UpdateAileron(aileron);

                // model.func?
            }
        }

        private double throttle;
        public double VM_Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                model.UpdateThrottle(throttle);
                // model.func?
            }
        }



        public void VMUpadteThrottle(double value)
        {
            model.UpdateThrottle(value);
        }

        public void VMUpadteAileron(double value)
        {
            model.UpdateAileron(value);
        }

        





    }
}
