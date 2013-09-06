using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Threading;
using Windows.Foundation;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Buffer = System.Buffer;

namespace RobotKit.Internal
{
    public class RobotSession
    {
        private Int32 _seqNum = 0;

        private ResponseParser parser;

        private Queue<DeviceMessage> sentMessages = new Queue<DeviceMessage>();

        private StreamSocket _socket;

        private Dispatcher _coreDispatcher;

        public RobotSession.RobotResponseDelegate _responseDelegate;
        private PeerInformation _peerInfo;

        public RobotSession(Robot robot)
        {
            parser = new ResponseParser(robot);
        }

        public RobotSession(PeerInformation peerInfo, Robot robot, Dispatcher coreDispatcher)
        {
            _peerInfo = peerInfo;
            parser = new ResponseParser(robot);
            _coreDispatcher = coreDispatcher;
        }

        public async Task Disconnect()
        {
            if (_socket != null)
            {
                await _socket.OutputStream.FlushAsync();
                _socket.Dispose();
                _socket = null;
            }
        }

        private void DispatchResponseThreadSafe(Response response)
        {
            if (_responseDelegate != null)
            {
                var coreDispatcher = _coreDispatcher;
                coreDispatcher.BeginInvoke(() => _responseDelegate(response));
            }
        }

        public async Task<Boolean> Initialize()
        {
            Boolean flag;
            _socket = new StreamSocket();
            try
            {
                await _socket.ConnectAsync(_peerInfo.HostName, "1");
               PostSocketRead(16);
                flag = true;
                return flag;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(String.Concat("Exception connecting: ", exception.Message));
            }
            flag = false;
            return flag;
        }

        public void OnDataReadCompletion(UInt32 bytesRead, DataReader readPacket)
        {
           
            if (readPacket == null)
            {
                Debug.WriteLine("DataReader is null");
            }
            else if (readPacket.UnconsumedBufferLength != 0)
            {
                Byte[] numArray = new Byte[bytesRead];
                readPacket.ReadBytes(numArray);
                Response response = parser.processRawBytes(numArray);
                if (response != null)
                {
                    DispatchResponseThreadSafe(response);
                }
                PostSocketRead(16);
            }
            else
            {
                Debug.WriteLine("Received zero bytes from the socket. Server must have closed the connection.");
                Debug.WriteLine("Try disconnecting and reconnecting to the server");
            }
            
        }

        private void PostSocketRead(Int32 length)
        {
            try
            {
                var buffer = new Byte[length];
                var asyncOperationWithProgress = _socket.InputStream.ReadAsync(buffer.AsBuffer(), (UInt32)length, InputStreamOptions.Partial);
                asyncOperationWithProgress.Completed += (info, status) =>
                {
                      switch (status)
                    {
                        case AsyncStatus.Completed:
                        case AsyncStatus.Error:
                        {
                            try
                            {
                                IBuffer results = info.GetResults();
                                OnDataReadCompletion(results.Length, DataReader.FromBuffer(results));
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine(String.Concat("Read operation failed:  ", exception.Message));
                            }
                            break;
                        }
                    }
                };
            }
            catch (Exception exception1)
            {
                Debug.WriteLine(String.Concat("failed to post a read failed with error:  ", exception1.Message));
            }
        }

        public async void WriteToRobot(DeviceMessage message)
        {
            RobotSession robotSession = this;
            Int32 num = robotSession._seqNum;
            Int32 num1 = num;
            robotSession._seqNum = num + 1;
            if (num1 >= 255)
            {
                _seqNum = 0;
            }
            message.setSequence((Byte)_seqNum);
            Byte[] data = message.Data;
            DataWriter dataWriter = new DataWriter();
            try
            {
                dataWriter.WriteBytes(data);
                await _socket.OutputStream.WriteAsync(dataWriter.DetachBuffer());
            }
            finally
            {
                if (dataWriter != null)
                {
                    dataWriter.Dispose();
                }
            }
        }

        public delegate void RobotResponseDelegate(Response response);
    }
}