using System;

namespace RobotKit
{
    [Flags]
    public enum DataStreaming : ulong
    {
        Gyrometer = 7168,
        Accelerometer = 57344,
        Attitude = 458752,
        Velocity = 108086391056891904,
        Location = 864691128455135232,
        Quaternion = 17293822569102704640
    }
}