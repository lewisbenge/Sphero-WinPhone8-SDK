using System;

namespace RobotKit
{
    internal class Value
    {
        public Value()
        {
        }

        public static Byte[] BytesForInt(Int32 iValue)
        {
            Byte num = (Byte)((iValue & 65280) >> 8);
            return new Byte[] { num, (Byte)(iValue & 255) };
        }

        public static Byte[] BytesForLong(Int64 lValue)
        {
            Byte[] numArray = new Byte[] { (Byte)(lValue >> 24), (Byte)(lValue >> 16), (Byte)(lValue >> 8), (Byte)lValue };
            return numArray;
        }

        public static Int32 clamp(Int32 value, Int32 min, Int32 max)
        {
            Int32 num;
            if (value <= max)
            {
                num = (value >= min ? value : min);
            }
            else
            {
                num = max;
            }
            return num;
        }

        public static Int32 IntForBytes(Byte lsb, Byte msb)
        {
            Byte[] numArray = new Byte[] { lsb, msb };
            return BitConverter.ToUInt16(numArray, 0);
        }
    }
}