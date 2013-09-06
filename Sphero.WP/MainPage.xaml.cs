using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RobotKit;

namespace Sphero.WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
          
            var provider = RobotProvider.GetSharedProvider();
       
            provider.DiscoveredRobotEvent += (o, robot) => provider.ConnectRobot(robot);
            provider.ConnectedRobotEvent += (o, robot) =>
            {
                var s = (RobotKit.Sphero) robot;
                s.SetRGBLED(120,140,160);


            };
            provider.FindRobots();
        }

      
      
    }
}