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
        bool GetBoolRunning();


    }
    class MainModel : IMainModel
    {
        ITelnetClient telnetClient;
        volatile bool stopRunning;
        public bool GetBoolRunning()
        {
            return this.stopRunning;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// constructor.
        /// initialize the telnet client with  a given one, and set the flag of main loop to false.
        /// </summary>
        /// <param name="client">the given client</param>
        public MainModel(ITelnetClient client)
        {
            this.telnetClient = client;
            this.stopRunning = true;
        }



        /// <summary>
        /// connecting the telnet client to a server.
        /// </summary>
        /// <param name="ip">the ip address of the server.</param>
        /// <param name="port">the port number of the server.</param>
        public void connect(string ip, int port)
        {
            StrException = "trying to connect...\npleast wait";

            bool tryToConnect = false;
            for(int i=0; i<2; i++)
            {
                try
                {
                    this.stopRunning = false;
                    this.telnetClient.connect(ip, port);
                    StrException = "connected to the server succesfuly";
                    this.start();
                    tryToConnect = true;
                    break;
                }
                catch
                {
                    StrException = "trying to connect...\npleast wait";
                    Console.WriteLine("can't connect to the serverXXX");
                }
            }
            if(!tryToConnect)
            {
                StrException = "can't connect to the server";
                stopRunning = true;

            }

        }
        /// <summary>
        /// disconnecting from the server.
        /// </summary>
        public void disconnect()
        {
            this.stopRunning = true;
            this.telnetClient.disconnect();
            this.telnetClient = new MyTelnetClient();
        }
        /// <summary>
        /// the main loop - reading data from the server 5 times per seccond.
        /// </summary>
        double tempLatitude;
        double tempLongitude;
        int errorCounter = 0;
        public void start()
        {
            new Thread(delegate ()
            {
                
                while (!stopRunning)
                {
                    try
                    {
                        string tempStr;
                        try
                        {
                            telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n"); //1
                                                                                                                  //HeadingDeg = Double.Parse(telnetClient.read());
                            tempStr = telnetClient.read();
                            //Console.WriteLine("\nheading value="+ telnetClient.read());
                            HeadingDeg = Double.Parse(tempStr);

                            //Console.WriteLine("1, " + a);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of heading";
                            //Console.WriteLine("heading, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");//2
                            tempStr = telnetClient.read();
                            GpsVerticalSpeed = Double.Parse(tempStr);
                            //errorCounter = 0;
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of vertical speed";
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");//3
                            tempStr = telnetClient.read();
                            GpsGroundSpeed = Double.Parse(tempStr);
                            //errorCounter = 0;
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of GpsGroundSpeed";
                            //Console.WriteLine("GpsGroundSpeed, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");//4
                            tempStr = telnetClient.read();
                            AirSpeed = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of AirSpeed";
                            //Console.WriteLine("AirSpeed, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");//5
                            tempStr = telnetClient.read();
                            GpsAltitude = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of GpsAltitude";
                            //Console.WriteLine("GpsAltitude, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");//6
                            tempStr = telnetClient.read();
                            InternalRollDeg = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of InternalRollDeg";
                            //Console.WriteLine("InternalRollDeg, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");//7
                            tempStr = telnetClient.read();
                            InternalPitchDeg = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of InternalPitchDeg";
                            //Console.WriteLine("InternalPitchDeg, " + telnetClient.read());
                            errorCounter++;
                        }

                        try
                        {
                            telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");//8
                            tempStr = telnetClient.read();
                            //Console.WriteLine("altitude is: " + tempStr);
                            AltimeterAltitude = Double.Parse(tempStr);
                            //Console.WriteLine("altitude is: " + AltimeterAltitude);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of AltimeterAltitude";
                            //Console.WriteLine("AltimeterAltitude, " + telnetClient.read());
                            errorCounter++;
                        }

                        // update Latitude
                        try
                        {
                            telnetClient.write("get /position/latitude-deg\n"); // x value of the pin. (x,y)
                            tempStr = telnetClient.read();
                            tempLatitude = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of Latitude";
                            //Console.WriteLine("Latitude, " + telnetClient.read());
                            errorCounter++;
                        }
                        if ((tempLatitude >= -90) && (tempLatitude <= 90))
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

                        // update Longtitude
                        try
                        {
                            telnetClient.write("get /position/longitude-deg\n"); // y value of the pin. (x,y)
                            tempStr = telnetClient.read();
                            tempLongitude = Double.Parse(tempStr);
                        }
                        catch
                        {
                            StrException = "error - can't update the current value of Longtitude";
                            //Console.WriteLine("Longtitude, " + telnetClient.read());
                            errorCounter++;
                        }
                        if ((tempLongitude >= -180) && (tempLongitude <= 180))
                        {
                            Longtitude = tempLongitude;
                            //Console.WriteLine("longitude = " + longitude);
                        }
                        else
                        {
                            StrException = "longitude is not in the range";
                            //Console.WriteLine(StrException);
                        }
                        //Longtitude = Double.Parse(telnetClient.read());

                        Location = new Location(latitude, longitude);
                        // reads 5 times per second.
                        Thread.Sleep(200);
                    }
                    catch (System.IO.IOException e)
                    {
                        if(!stopRunning)
                        {
                            this.disconnect();
                            this.StrException = "problem with connecting the server\nplease try to connect again\n";
                            break;
                        } else
                        {
                            break;
                        }
                    }
                    //Console.WriteLine("counter is: " + this.errorCounter);
                    
                    //if(this.errorCounter==10)
                    //{
                    //    //this.telnetClient.disconnect();
                    //    this.disconnect();
                    //    this.StrException = "problem with connecting the server\nplease try to connect again\n";
                    //}
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
                NotifyPropertyChanged("AltimeterAltitude");
            }
        }

        public void UpdateThrottle(double value)
        {
            //string check = "abc";
            //double num = Double.Parse(check);
            if(!this.stopRunning)
            {
                try
                {
                    telnetClient.write("set /controls/engines/current-engine/throttle " + value + "\n");
                }
                catch
                {
                    errorCounter++;
                    this.StrException = "error - can't update information about Throttle";
                }
                try
                {
                    telnetClient.read();
                }
                catch
                {

                }
            } else
            {
                StrException = "you can't change the Throttle value\nfirst, you need to connect to the server\n";
            }
            
            // had to put it for the simulator from telegram
            //string b = telnetClient.read();
            //Console.WriteLine("3, "+b);
        }

        public void UpdateAileron(double value)
        {
            Console.WriteLine("\n1\n");

            // if you are connecting to the server
            if (!this.stopRunning)
            {
                Console.WriteLine("\n2\n");

                // try to work as usual
                try
                {
                    try
                    {
                        telnetClient.write("set /controls/flight/aileron " + value + "\n");
                        Console.WriteLine("\n3\n");

                    }
                    catch
                    {
                        errorCounter++;
                        this.StrException = "error - can't update information about Aileron";
                        Console.WriteLine("\n4\n");

                    }
                    try
                    {
                        telnetClient.read();
                    }
                    catch
                    {

                    }
                }
                catch (System.IO.IOException e)
                {
                    this.disconnect();
                    this.StrException = "problem with connecting the server\nplease try to connect again\n";
                    Console.WriteLine("\ndisconnecting\n");
                    //if (!stopRunning)
                    //{
                        
                            
                        
                    //}
                    //else
                    //{

                    //}
                }

            }
            else
            {
                StrException = "you can't change the Aileron value\nfirst, you need to connect to the server\n";
            }

        }

        public void UpdateRudder(double value)
        {
            if (!this.stopRunning)
            {
                try
                {
                    telnetClient.write("set /controls/flight/rudder " + (value / 84) + "\n");
                }
                catch
                {
                    errorCounter++;
                    this.StrException = "error - can't update information about Rudder";
                }
                // had to put it for the simulator from telegram
                try
                {
                    telnetClient.read();
                }
                catch
                {

                }
            } else
            {
                StrException = "you can't change the Jostick's values\nfirst, you need to connect to the server\n";
            }

        }
        
        public void UpdateElevator(double value)
        {
            if (!this.stopRunning)
            {
                try
                {
                    telnetClient.write("set /controls/flight/elevator " + (value / (-84)) + "\n");
                }
                catch
                {
                    errorCounter++;
                    this.StrException = "error - can't update information about Elevator";
                }
                // had to put it for the simulator from telegram
                try
                {
                    telnetClient.read();
                }
                catch
                {

                }
            } else
            {
                StrException = "you can't change the Jostick's values\nfirst, you need to connect to the server\n";
            }
        }
    }
    class MyTelnetClient : ITelnetClient
    {

        private TcpClient client;
        bool isConnected = false;
        NetworkStream stream;


        //string MyIP = "127.0.0.1";
        //int MyPort = 5402;

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

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Console.WriteLine("aaa");
            this.client.Connect(endPoint);
            Console.WriteLine("bbb");
            this.isConnected = true;
            Console.WriteLine("Server is connected.");

            this.client.ReceiveTimeout = 10000;
            this.client.SendTimeout = 1000;
            this.stream = client.GetStream();

            
        }

        /// <summary>
        /// closing the stream and the connection itself.
        /// </summary>
        public void disconnect()
        {
            //try
            //{
            //    this.client.GetStream().Close();
            //}
            //catch
            //{

            //}
            this.stream.Close();
            this.client.Close();
            //this.client = new TcpClient();
        }

        /// <summary>
        /// sending data to the server.
        /// </summary>
        /// <param name="userCommand">the command we want to send to the server.</param>
        public void write(string userCommand)
        {
            byte[] BytesArr = new byte[1024];
            //ASCIIEncoding aSCII = new ASCIIEncoding();
            //BytesArr = aSCII.GetBytes(userCommand); // encoding the user's command into the buffer.
            BytesArr = Encoding.ASCII.GetBytes(userCommand);
            try
            {
                this.stream.Write(BytesArr, 0, BytesArr.Length); //sending the data stored inside of BytesArr.
                this.stream.Flush(); // clean the stream.
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("\nProblem, the server is disconnected\n");
            }
            


        }
        /// <summary>
        /// reads data from the server.
        /// </summary>
        /// <returns>the answer from the server as string.</returns>
        public string read()
        {
            byte[] data = new byte[1024];
            String serverAnswer = String.Empty;
            //NetworkStream networkStream = this.client.GetStream();
            this.stream = this.client.GetStream();
            int bytes = this.stream.Read(data, 0, data.Length);
            serverAnswer = System.Text.Encoding.ASCII.GetString(data, 0, bytes);







            return serverAnswer;
        }


    }
}