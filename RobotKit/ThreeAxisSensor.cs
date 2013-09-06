using System;

namespace RobotKit
{
    public class ThreeAxisSensor
    {
        public readonly Single X;

        public readonly Single Y;

        public readonly Single Z;

        public ThreeAxisSensor(Single x, Single y, Single z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}