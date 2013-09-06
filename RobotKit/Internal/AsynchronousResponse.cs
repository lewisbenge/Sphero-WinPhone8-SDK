using System;

namespace RobotKit.Internal
{
    public class AsynchronousResponse : Response
    {
        public readonly AsynchronousId AsyncId;

        public readonly Int32 PacketLength;

        internal AsynchronousResponse(Robot robot, ResponseCode code, Byte seqNum, Byte[] data, AsynchronousId asyncId, Int32 length)
            : base(robot, code, seqNum, data)
        {
            AsyncId = asyncId;
            PacketLength = length;
        }
    }
}