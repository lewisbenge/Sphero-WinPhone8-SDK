using System;

namespace RobotKit
{
    public class TwoAxisSensor
    {
        public readonly Single X;

        public readonly Single Y;

        public TwoAxisSensor(Single x, Single y)
        {
            X = x;
            Y = y;
        }
    }
}