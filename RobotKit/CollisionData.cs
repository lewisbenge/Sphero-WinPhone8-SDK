using System;
using RobotKit.Internal;

namespace RobotKit
{
    public class CollisionData : AsynchronousResponse
    {
        public readonly AccelerometerReading CollisionAccelerometerReading;

        public readonly TwoAxisSensor CollisionPower;

        public readonly Single SpheroSpeed;

        public readonly Int32 Timestamp;

        public readonly Boolean XAxisCollision;

        public readonly Boolean YAxisCollision;

        public CollisionData(Robot robot, ResponseCode code, Byte seqNum, Byte[] data, AsynchronousId asyncCode, Int32 length)
            : base(robot, code, seqNum, data, asyncCode, length)
        {
            if ((Int32)Data.Length == 16)
            {
                CollisionAccelerometerReading = new AccelerometerReading((Single)GetFloatFromBytes(0), (Single)GetFloatFromBytes(2), (Single)GetFloatFromBytes(4));
                XAxisCollision = (Data[6] & 1) > 0;
                YAxisCollision = (Data[6] >> 1 & 1) > 0;
                CollisionPower = new TwoAxisSensor((Single)GetFloatFromBytes(7), (Single)GetFloatFromBytes(9));
                SpheroSpeed = (Single)Data[11] / 255f;
                Timestamp = GetIntFromBytes((Int32)Data[12]);
            }
        }

        private Int32 GetFloatFromBytes(Int32 location)
        {
            Int32 data = (Int16)(Data[location] << 8) | Data[location + 1];
            return data;
        }

        private Int32 GetIntFromBytes(Int32 location)
        {
            Int32 data = Data[location] << 24 | Data[location + 1] << 16 | Data[location + 2] << 8 | Data[location + 3];
            return data;
        }
    }
}