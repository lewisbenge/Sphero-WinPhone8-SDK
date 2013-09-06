using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RobotKit.Internal
{
    public class ResponseParser
    {
        private Robot _robot;

        private List<byte> _rawData = new List<byte>();

        public ResponseParser(Robot robot)
        {
            _robot = robot;
        }

        public ResponseParser()
        {
        }

        private Int32 checksumFor(Byte[] packet)
        {
            Int32 num = 0;
            for (Int32 i = 2; i < (Int32)packet.Length - 1; i++)
            {
                num = num + packet[i];
            }
            return num;
        }

        private Response parseACK(Byte[] rawAckPacket)
        {
            Debug.WriteLine(String.Concat("ACK raw=", DebugGoodies.ByteArrayToString(rawAckPacket)));
            Byte num = rawAckPacket[2];
            Byte num1 = rawAckPacket[3];
            Byte num2 = rawAckPacket[4];
            Byte num3 = rawAckPacket[(Int32)rawAckPacket.Length - 1];
            checksumFor(rawAckPacket);
            return new Response(_robot, (ResponseCode)num, num1, null);
        }

        private Response parseAsync()
        {
            Response asyncSensorDatum;
            Byte item = _rawData[2];
            Byte num = _rawData[3];
            Byte item1 = _rawData[4];
            Int32 num1 = Value.IntForBytes(item1, num) - 1;
            Int32 num2 = num1 + 6;
            if (_rawData.Count >= num2)
            {
                Byte[] numArray = new Byte[num1];
                _rawData.CopyTo(5, numArray, 0, num1);
                _rawData.RemoveRange(0, num2);
                if (item == 3)
                {
                    asyncSensorDatum = new AsyncSensorData(_robot, ResponseCode.OK, 0, numArray, AsynchronousId.SENSOR_DATA_STREAMING, num1, _robot.SensorControl.Mask);
                }
                else if (item != 7)
                {
                    asyncSensorDatum = null;
                }
                else
                {
                    asyncSensorDatum = new CollisionData(_robot, ResponseCode.OK, 0, numArray, AsynchronousId.COLLISION_DETECTED, num1);
                }
            }
            else
            {
                asyncSensorDatum = null;
            }
            return asyncSensorDatum;
        }

        public Response processRawBytes(Byte[] ba)
        {
            Response response;
            Byte[] numArray = ba;
            for (Int32 i = 0; i < (Int32)numArray.Length; i++)
            {
                Byte num = numArray[i];
                _rawData.Add(num);
            }
            if (_rawData.Count >= 6)
            {
                Debug.WriteLine(String.Concat("IN:  ", DebugGoodies.ByteArrayToString(_rawData)));
                if ((_rawData[0] != 255 ? true : _rawData[1] != 255))
                {
                    if ((_rawData[0] != 255 ? true : _rawData[1] != 254))
                    {
                        throw new InvalidResponseException("Responses must be initiated with 0xFF", ba);
                    }
                    response = parseAsync();
                }
                else
                {
                    Byte item = _rawData[2];
                    Byte item1 = _rawData[3];
                    Int32 num1 = 5 + _rawData[4];
                    if (_rawData.Count >= num1)
                    {
                        Object[] objArray = new Object[] { "ACK rc={0:x2} seq=", item1, " len=", num1 };
                        String str = String.Concat(objArray);
                        objArray = new Object[] { item };
                        Debug.WriteLine(str, objArray);
                        Byte[] numArray1 = new Byte[num1];
                        _rawData.CopyTo(0, numArray1, 0, num1);
                        _rawData.RemoveRange(0, num1);
                        response = parseACK(numArray1);
                    }
                    else
                    {
                        response = null;
                    }
                }
            }
            else
            {
                response = null;
            }
            return response;
        }

        public Int32 Remainder()
        {
            return _rawData.Count;
        }
    }
}