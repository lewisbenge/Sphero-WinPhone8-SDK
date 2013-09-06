using System;

namespace RobotKit
{
    public class GyrometerReading : ThreeAxisSensor
    {
        private const Single kDpsConv = 0.1f;

        public GyrometerReading(Single x, Single y, Single z) : base(x * 0.1f, y * 0.1f, z * 0.1f)
        {
        }
    }
}