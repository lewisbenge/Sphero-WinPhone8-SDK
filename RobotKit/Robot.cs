using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Proximity;
using Windows.UI.Core;
using RobotKit.Internal;

namespace RobotKit
{
    public class Robot
    {
        public Robot.ConnectionStateChanged OnConnectionStateChanged;

        protected ConnectionState _connectionState = ConnectionState.Disconnected;

        private RobotSession _session = null;

        private String _btname = null;

        private String _givenName = null;

        private PeerInformation _info = null;

        private SensorControl _sensorControl;

        private CollisionControl _collisionControl;

    

        public String BluetoothName
        {
            get
            {
                return _btname;
            }
        }

        public CollisionControl CollisionControl
        {
            get
            {
                return _collisionControl;
            }
        }

        public ConnectionState ConnectionState
        {
            get
            {
                return _connectionState;
            }
        }

        public String Name
        {
            get
            {
                String str;
                str = (_givenName != null ? _givenName : _btname);
                return str;
            }
        }

        public SensorControl SensorControl
        {
            get
            {
                return _sensorControl;
            }
        }

        internal Robot(PeerInformation info)
        {
            _info = info;
            _btname = info.DisplayName;
        }

        internal async Task<Boolean> Connect()
        {
            Boolean flag;
            Debug.WriteLine(String.Concat("Connecting: ", _info.DisplayName));
            try
            {
            
                if (_info == null)
                {
                    Debug.WriteLine(String.Concat("Unable to build Service for ", this));
                    flag = false;
                    return flag;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;Debug.WriteLine(String.Concat("Exception looking up service ", _info.DisplayName, " ", exception.Message));
                flag = false;
                return flag;
            }
            var dispatcher = Deployment.Current.Dispatcher;
            _session = new RobotSession(_info, this, dispatcher);
            if (!await _session.Initialize())
            {
                flag = false;
            }
            else
            {
                InternalSetConnectionState(ConnectionState.Connected);
                Debug.WriteLine(String.Concat("Connected: ", ToString()));
                _sensorControl = new SensorControl(_session);
                _collisionControl = new CollisionControl(_session);
                _session._responseDelegate = new RobotSession.RobotResponseDelegate(OnResponseReceived);
                flag = true;
            }
            return flag;
        }

        public async void Disconnect()
        {
            await _session.Disconnect();
            if (_info != null)
            {
                _info = null;
            }
        }

        private void InternalSetConnectionState(ConnectionState newState)
        {
            Debug.WriteLine(String.Concat(_btname, " is ", newState));
            if (newState != _connectionState)
            {
                _connectionState = newState;
                if (OnConnectionStateChanged != null)
                {
                    OnConnectionStateChanged(this, newState);
                }
            }
        }

        private void OnResponseReceived(Response response)
        {
            if (response is AsyncSensorData)
            {
                _sensorControl.ReceiveSensorData((AsyncSensorData)response);
            }
            else if (response is CollisionData)
            {
                _collisionControl.ReceiveCollisionData((CollisionData)response);
            }
        }

        public void Sleep(Int32 wakeup, Byte macro = 0)
        {
            throw new NotImplementedException("due July 2013");
        }

        public void Sleep(Int32 wakeup, Int32 orbBasicLineNum = 0)
        {
            throw new NotImplementedException("due July 2013");
        }

        public void Sleep(Int32 wakeup = 0, Byte macro = 0, Int32 orbBasic = 0)
        {
            Byte[] numArray = new Byte[5];
            WriteToRobot(new DeviceMessage(0, 34, numArray, DeviceMessage.SopParameters.ResetTimeout));
        }

        public override String ToString()
        {
            Object[] connectionServiceName = new Object[] { "[Robot ", _btname, " @ ", _info.HostName, " ", _connectionState, "]" };
            return String.Concat(connectionServiceName);
        }

        public void WriteName(String name)
        {
            name = name.Trim();
            Byte[] bytes = Encoding.UTF8.GetBytes(name);
            WriteToRobot(new DeviceMessage(0, 16, bytes, DeviceMessage.SopParameters.ResetTimeout));
        }

        public void WriteToRobot(DeviceMessage msg)
        {
            if (ConnectionState == ConnectionState.Connected)
            {
                _session.WriteToRobot(msg);
            }
        }

        public delegate void ConnectionStateChanged(Robot sphero, ConnectionState newState);
    }
}