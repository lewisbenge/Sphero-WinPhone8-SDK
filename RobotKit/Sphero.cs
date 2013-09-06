using System;
using Windows.Networking.Proximity;
using RobotKit.Internal;

namespace RobotKit
{
    public class Sphero : Robot
    {
        internal Sphero(PeerInformation info) : base(info)
        {
        }

        public void Roll(Int32 heading, Single speed)
        {
            Byte num = (Byte)(speed * 255f);
            Byte[] numArray = Value.BytesForInt(Value.clamp(heading, 0, 359));
            Byte[] numArray1 = new Byte[] { num, numArray[0], numArray[1], 1 };
            DeviceMessage deviceMessage = new DeviceMessage(2, 48, numArray1, DeviceMessage.SopParameters.ResetTimeout);
            deviceMessage.NoResponse();
            base.WriteToRobot(deviceMessage);
        }

        public void SetBackLED(Single intensity)
        {
            Byte[] numArray = new Byte[] { (Byte)(intensity * 255f) };
            base.WriteToRobot(new DeviceMessage(2, 33, numArray, DeviceMessage.SopParameters.ResetTimeout));
        }

        public void SetHeading(Int32 heading)
        {
            Byte num = (Byte)((heading & 65280) >> 8);
            Byte num1 = (Byte)(heading & 255);
            Byte[] numArray = new Byte[] { num, num1 };
            base.WriteToRobot(new DeviceMessage(2, 1, numArray, DeviceMessage.SopParameters.ResetTimeout));
        }

        public void SetRGBLED(Int32 red, Int32 green, Int32 blue)
        {
            red = Value.clamp(red, 0, 255);
            green = Value.clamp(green, 0, 255);
            blue = Value.clamp(blue, 0, 255);
            Byte[] numArray = new Byte[] { (Byte)red, (Byte)green, (Byte)blue, 1 };
            DeviceMessage deviceMessage = new DeviceMessage(2, 32, numArray, DeviceMessage.SopParameters.ResetTimeout);
            deviceMessage.NoResponse();
            base.WriteToRobot(deviceMessage);
        }
    }
}