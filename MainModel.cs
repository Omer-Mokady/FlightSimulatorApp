using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{

    interface ITelnetClient
    {
        void connect(string ip, int port);
        void disconnect();
        void write(string userCommand);
        string read();
    }
    interface IMainModel : INotifyPropertyChanged
    {
        //connection.
        void connect(string ip, int port);
        void disconnect();
        void start(); // main loop.

        //Properties - add the missing
        double HeadingDeg { get; set; }
        double GpsVerticalSpeed { get; set; }
        double GpsGroundSpeed { get; set; }
        double AirSpeed { get; set; }
        double GpsAltitude { get; set; }
        double InternalRollDeg { get; set; }
        double InternalPitchDeg { get; set; }
        double AltimeterAltitude { get; set; }
        double Latitude { get; set; }
        double Longtitude { get; set; }
        Location Location { get; set; }
        string StrException { get; set; }
        // activators
        void UpdateThrottle(double value);
        void UpdateAileron(double value);
        void UpdateRudder(double value);
        void UpdateElevator(double value);


    }
    class MainModel : IMainModel
    {
        ITelnetClient telnetClient;
        volatile bool stopRunning;
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// constructor.
        /// initialize the telnet client with  a given one, and set the flag of main loop to false.
        /// </summary>
        /// <param name="client">the given client</param>
        public MainModel(ITelnetClient client)
        {
            this.telnetClient = client;
            this.stopRunning = false;
        }
        /// <summary>
        /// connecting the telnet client to a server.
        /// </summary>
        /// <param name="ip">the ip address of the server.</param>
        /// <param name="port">the port number of the server.</param>
        public void connect(string ip, int port)
        {
            this.telnetClient.connect(ip, port);
        }
        /// <summary>
        /// disconnecting from the server.
        /// </summary>
        public void disconnect()
        {
            this.stopRunning = true;
            this.telnetClient.disconnect();
        }
        /// <summary>
        /// the main loop - reading data from the server 5 times per seccond.
        /// </summary>
        double tempLatitude;
        double tempLongitude;
        public void start()
        {
            new Thread(delegate ()
            {
                while (!stopRunning)
                {
                    //Console.WriteLine("inside Start loop - main model");
                    telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n"); //1
                    HeadingDeg = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");//2
                    GpsVerticalSpeed = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");//3
                    GpsGroundSpeed = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");//4
                    AirSpeed = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");//5
                    GpsAltitude = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");//6
                    InternalRollDeg = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");//7
                    InternalPitchDeg = Double.Parse(telnetClient.read());

                    telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");//8
                    AltimeterAltitude = Double.Parse(telnetClient.read());

                    telnetClient.write("get /position/latitude-deg\n"); // x value of the pin. (x,y)
                    tempLatitude = Double.Parse(telnetClient.read());
                    if ((tempLatitude>=-90) && (tempLatitude<=32.06))
                    {
                        Latitude = tempLatitude;
                        //Console.WriteLine("latitude = " + Latitude);
                    }
                    else
                    {
                        StrException = "altitude is not in the range";
                        //Console.WriteLine(StrException);
                    }
                    //Latitude = Double.Parse(telnetClient.read());

                    telnetClient.write("get /position/longitude-deg\n"); // y value of the pin. (x,y)
                    tempLongitude = Double.Parse(telnetClient.read());
                    if ((tempLongitude >= -180) && (tempLongitude <= 35))
                    {
                        Longtitude = tempLongitude;
                        Console.WriteLine("longitude = " + longitude);
                    } else
                    {
                        StrException = "longitude is not in the range";
                        //Console.WriteLine(StrException);
                    }
                    //Longtitude = Double.Parse(telnetClient.read());

                    Location = new Location(latitude, longitude);
                    // reads 5 times per second.
                    Thread.Sleep(200);
                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private double latitude;
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                NotifyPropertyChanged("Latitude");
            }
        }

        private string strException;
        public string StrException
        {
            get
            {
                return strException;
            }
            set
            {
                strException = value;
                NotifyPropertyChanged("StrException");
            }
        }

        private double longitude;
        public double Longtitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                NotifyPropertyChanged("Longtitude");
            }
        }

        private Location location;
        public Location Location
        {
            get
            {
                return location;
            }
            set
            {
                //Console.WriteLine("MainModel->Location->set " + value);
                location = value;
                NotifyPropertyChanged("Location");
            }
        }

        private double headingDeg;
        public double HeadingDeg
        {
            get
            {
                return headingDeg;
            }
            set
            {
                headingDeg = value;
                // ??????????????????????????????????????????????
                //Console.WriteLine("HeadingDeg " + value);
                NotifyPropertyChanged("HeadingDeg");
            }
        }

        private double gpsVerticalSpeed;
        public double GpsVerticalSpeed
        {
            get
            {
                return gpsVerticalSpeed;
            }
            set
            {
                gpsVerticalSpeed = value;
                //Console.WriteLine("GpsVerticalSpeed " + value);
                NotifyPropertyChanged("GpsVerticalSpeed");
            }
        }

        private double gpsGroundSpeed;
        public double GpsGroundSpeed
        {
            get
            {
                return gpsGroundSpeed;
            }
            set
            {
                gpsGroundSpeed = value;
                //Console.WriteLine("GpsGroundSpeed " + value);
                NotifyPropertyChanged("GpsGroundSpeed");
            }
        }
        private double airSpeed;
        public double AirSpeed
        {
            get
            {
                return airSpeed;
            }
            set
            {
                airSpeed = value;
                //Console.WriteLine("AirSpeed " + value);
                NotifyPropertyChanged("AirSpeed");
            }
        }

        private double gpsAltitude;
        public double GpsAltitude
        {
            get
            {
                return gpsAltitude;
            }
            set
            {
                gpsAltitude = value;
                //Console.WriteLine("GpsAltitude " + value);
                NotifyPropertyChanged("GpsAltitude");
            }
        }

        private double internalRollDeg;
        public double InternalRollDeg
        {
            get
            {
                return internalRollDeg;
            }
            set
            {
                internalRollDeg = value;
                //Console.WriteLine("InternalRollDeg " + value);
                NotifyPropertyChanged("InternalRollDeg");
            }
        }

        private double internalPitchDeg;
        public double InternalPitchDeg
        {
            get
            {
                return internalPitchDeg;
            }
            set
            {
                internalPitchDeg = value;
                //Console.WriteLine("InternalPitchDeg " + value);
                NotifyPropertyChanged("InternalPitchDeg");
            }
        }

        private double altimeterAltitude;
        public double AltimeterAltitude
        {
            get
            {
                return altimeterAltitude;
            }
            set
            {
                altimeterAltitude = value;
                NotifyPropertyChanged("AltimeterAltitude " + value);
            }
        }

        public void UpdateThrottle(double value)
        {
            telnetClient.write("set /controls/engines/current-engine/throttle " + value + "\n");
            // had to put it for the simulator from telegram
            telnetClient.read();
        }

        public void UpdateAileron(double value)
        {
            telnetClient.write("set /controls/flight/aileron " + value + "\n");
            // had to put it for the simulator from telegram
            telnetClient.read();
        }

        public void UpdateRudder(double value)
        {
            telnetClient.write("set /controls/flight/rudder " + value + "\n");
            // had to put it for the simulator from telegram
            telnetClient.read();
        }
        
        public void UpdateElevator(double value)
        {
            telnetClient.write("set /controls/flight/elevator " + value + "\n");
            // had to put it for the simulator from telegram
            telnetClient.read();
        }
    }
    class MyTelnetClient : ITelnetClient
    {

        private TcpClient client;
        bool isConnected = false;


        string defaultIP = "127.0.0.1";
        int defaultPort = 5402;

        /// <summary>
        /// default Constructor.
        /// </summary>
        public MyTelnetClient()
        {
            this.client = new TcpClient();
        }

        /// <summary>
        /// connecting to a given port and ip.
        /// </summary>
        /// <param name="ip">the server's ip address.</param>
        /// <param name="port">the server's port number.</param>
        public void connect(string ip, int port)
        {
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(defaultIP), defaultPort);
            //Console.WriteLine("inside client");
            //while the server is connected.
            while (!isConnected)
            {
                try
                {
                    this.client.Connect(endPoint);
                    this.isConnected = true;
                    //Console.WriteLine("Server is connected.");
                }
                catch (Exception)
                {
                    //Console.WriteLine("Couldn't connect to server.");
                }
            }
        }

        /// <summary>
        /// closing the stream and the connection itself.
        /// </summary>
        public void disconnect()
        {
            this.client.GetStream().Close();
            this.client.Close();
        }

        /// <summary>
        /// sending data to the server.
        /// </summary>
        /// <param name="userCommand">the command we want to send to the server.</param>
        public void write(string userCommand)
        {
            byte[] dataToSend = new byte[1024];
            // byte[] BytesArr;
            byte[] BytesArr = new byte[1024];
            NetworkStream networkStream = this.client.GetStream();
            ASCIIEncoding aSCII = new ASCIIEncoding();
            BytesArr = aSCII.GetBytes(userCommand); // encoding the user's command into the buffer.
            networkStream.Write(BytesArr, 0, BytesArr.Length); //sending the data stored inside of BytesArr.
            networkStream.Flush(); // clean the stream.
        }
        /// <summary>
        /// reads data from the server.
        /// </summary>
        /// <returns>the answer from the server as string.</returns>
        public string read()
        {
            byte[] data = new byte[1024];
            String serverAnswer = String.Empty;
            Int32 bytes = this.client.GetStream().Read(data, 0, data.Length);
            serverAnswer = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return serverAnswer;
        }


    }
}