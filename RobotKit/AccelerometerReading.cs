using System;

namespace RobotKit
{
    public class AccelerometerReading : ThreeAxisSensor
    {
        private const Single kGrav = 4096f;

        public AccelerometerReading(Single x, Single y, Single z)
            : base(x / 4096f, y / 4096f, z / 4096f)
        {
        }
    }
}
