using System;
using RobotKit.Internal;

namespace RobotKit
{
    public class AsyncSensorData : AsynchronousResponse
    {
        public readonly AttitudeReading Attitude;

        public readonly AccelerometerReading Accelorometer;

        public readonly GyrometerReading Gyrometer;

        public readonly QuaternionReading Quaternion;

        public readonly LocationReading Location;

        public readonly VelocityReading Velocity;

        public AsyncSensorData(Robot robot, ResponseCode code, Byte seqNum, Byte[] data, AsynchronousId asyncCode, Int32 length, UInt64 mask)
            : base(robot, code, seqNum, data, asyncCode, length)
        {
            if ((Int32)Data.Length == CountBits(mask) * 2)
            {
                Int32 num = 0;
                if (MaskHasFlag(mask, (DataStreaming)((Int64)458752)))
                {
                    Attitude = new AttitudeReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2), (Single)GetFloatFromBytes(num + 4));
                    num = num + 6;
                }
                if (MaskHasFlag(mask, (DataStreaming)((Int64)57344)))
                {
                    Accelorometer = new AccelerometerReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2), (Single)GetFloatFromBytes(num + 4));
                    num = num + 6;
                }
                if (MaskHasFlag(mask, (DataStreaming)((Int64)7168)))
                {
                    Gyrometer = new GyrometerReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2), (Single)GetFloatFromBytes(num + 4));
                    num = num + 6;
                }
                if (MaskHasFlag(mask, DataStreaming.Quaternion))
                {
                    Quaternion = new QuaternionReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2), (Single)GetFloatFromBytes(num + 4), (Single)GetFloatFromBytes(num + 6));
                    num = num + 8;
                }
                if (MaskHasFlag(mask, DataStreaming.Location))
                {
                    Location = new LocationReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2));
                    num = num + 4;
                }
                if (MaskHasFlag(mask, DataStreaming.Velocity))
                {
                    Velocity = new VelocityReading((Single)GetFloatFromBytes(num), (Single)GetFloatFromBytes(num + 2));
                    num = num + 4;
                }
            }
        }

        private Int32 CountBits(UInt64 mask)
        {
            Int32 num = 0;
            for (Int32 i = 0; i < 64; i++)
            {
                if ((mask >> (i & 63) & (Int64)1) > (Int64)0)
                {
                    num++;
                }
            }
            return num;
        }

        private Int32 GetFloatFromBytes(Int32 location)
        {
            Int32 data = (Int16)(Data[location] << 8) | Data[location + 1];
            return data;
        }

        private Boolean MaskHasFlag(UInt64 mask, DataStreaming flag)
        {
            return (mask & (UInt64)flag) > (Int64)0;
        }
    }
}