using System;
using System.Collections.Generic;
using System.Text;

namespace RobotKit
{
    internal class DebugGoodies
    {
        public DebugGoodies()
        {
        }

        public static String ByteArrayToString(Byte[] ba)
        {
            StringBuilder stringBuilder = new StringBuilder((Int32) ba.Length*2);
            Int32 num = 0;
            Byte[] numArray = ba;
            for (Int32 i = 0; i < (Int32) numArray.Length; i++)
            {
                Object[] objArray = new Object[] {numArray[i]};
                stringBuilder.AppendFormat("{0:x2}", objArray);
                Int32 num1 = num + 1;
                num = num1;
                if (num1%2 == 0)
                {
                    stringBuilder.AppendFormat(" ", new Object[0]);
                }
            }
            return stringBuilder.ToString();
        }

        public static String ByteArrayToString(List<Byte> rawData)
        {
            return DebugGoodies.ByteArrayToString(rawData.ToArray());
        }
    }
}