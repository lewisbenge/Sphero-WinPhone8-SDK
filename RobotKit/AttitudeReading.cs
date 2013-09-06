using System;

namespace RobotKit
{
    public class AttitudeReading
    {
        public readonly Single Roll;

        public readonly Single Pitch;

        public readonly Single Yaw;

        public AttitudeReading(Single roll, Single pitch, Single yaw)
        {
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
        }
    }
}