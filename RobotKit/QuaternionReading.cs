using System;

namespace RobotKit
{
    public class QuaternionReading
    {
        public readonly Single W;

        public readonly Single X;

        public readonly Single Y;

        public readonly Single Z;

        public QuaternionReading(Single w, Single x, Single y, Single z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }
    }
}