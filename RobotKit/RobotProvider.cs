using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Windows.Networking.Proximity;
using Microsoft.Phone.Shell;

namespace RobotKit
{
    public class RobotProvider
    {
        private static RobotProvider _sharedProvider;

        private List<Robot> pairedRobots = new List<Robot>();

        private List<Robot> connectedRobots = new List<Robot>();

        private Boolean _adapterEnabled = true;

        static RobotProvider()
        {
            RobotProvider._sharedProvider = null;
        }

        private RobotProvider()
        {
           
        }

        public async void ConnectRobot(Robot robot)
        {
            if (!await robot.Connect())
            {
                toastFailConnect(robot.BluetoothName);
            }
            else
            {
                EventHandler<Robot> connectedRobotEvent = ConnectedRobotEvent;
                if (connectedRobotEvent != null)
                {
                    connectedRobotEvent.Invoke(this, robot);
                }
                connectedRobots.Add(robot);
                toastConnect(robot.BluetoothName);
            }
        }

        public void DisconnectAll()
        {
            foreach (Robot connectedRobot in connectedRobots)
            {
                connectedRobot.Disconnect();
            }
        }

        public async void FindRobots()
        {
            Boolean flag;
            try
            {
                PeerFinder.AllowBluetooth = true;
                PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = ""; 
                var deviceInformationCollection = await PeerFinder.FindAllPeersAsync();

                if (!deviceInformationCollection.Any())
                {
                    EventHandler noRobotsEvent = NoRobotsEvent;
                    if (noRobotsEvent != null)
                    {
                        noRobotsEvent.Invoke(this, null);
                    }
                }
                foreach(var item in deviceInformationCollection)
                {
                    if (!item.DisplayName.Contains("Sphero"))
                    {
                        Debug.WriteLine("There needs to be a permission in the app manifest.");
                        Debug.WriteLine("Add UUID of Sphero to manifest: 00001101-0000-1000-8000-00805F9B34FB");
                    }
                    else
                    {
                        Sphero sphero = new Sphero(item);
                        EventHandler<Robot> discoveredRobotEvent = DiscoveredRobotEvent;
                        if (discoveredRobotEvent != null)
                        {
                            discoveredRobotEvent.Invoke(this, sphero);
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                if ((uint)exception.HResult == 0x8007048F)
                {
                    MessageBox.Show("Bluetooth is currently switched off");
                }

                Debug.WriteLine(String.Concat("Rfcomm Serial Service failed: ", exception));
            }
        }

        public Robot GetConnectedRobot()
        {
            return connectedRobots.First<Robot>();
        }

        public List<Robot> GetConnectedRobots()
        {
            return connectedRobots;
        }

        public static RobotProvider GetSharedProvider()
        {
            if (RobotProvider._sharedProvider == null)
            {
                RobotProvider._sharedProvider = new RobotProvider();
            }
            return RobotProvider._sharedProvider;
        }
        

        private void toastConnect(String name)
        {
            MessageBox.Show(String.Concat("Connected ", name),"Let's Go Ballin'!", MessageBoxButton.OK);
        }

        private void toastFailConnect(String name)
        {
            MessageBox.Show(String.Concat("Failed to Connect ", name), "Booooo!", MessageBoxButton.OK);
        }

        public event EventHandler<Robot> ConnectedRobotEvent;

        public event EventHandler<Robot> DiscoveredRobotEvent;

        public event EventHandler NoRobotsEvent;
    }
}