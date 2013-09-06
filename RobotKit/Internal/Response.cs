using System;

namespace RobotKit.Internal
{
    public class Response
    {
        public readonly ResponseCode RspCode;

        public readonly Byte SeqNum;

        public readonly Byte[] Data;

        public readonly Robot Robot;

        internal Response(Robot robot, ResponseCode code, Byte seqNum, Byte[] data)
        {
            Robot = robot;
            RspCode = code;
            SeqNum = seqNum;
            Data = data;
        }
    }
}