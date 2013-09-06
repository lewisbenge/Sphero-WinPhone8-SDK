using System;

namespace RobotKit.Internal
{
    internal class InvalidResponseException : Exception
    {
        internal InvalidResponseException(String msg, Byte[] theInvalidArray)
            : base(String.Concat(msg, " data: ", DebugGoodies.ByteArrayToString(theInvalidArray)))
        {
        }
    }
}