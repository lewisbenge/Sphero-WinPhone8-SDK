using System;
using RobotKit.Internal;

namespace RobotKit
{
    public class CollisionControl
    {
        private RobotSession _robotSession;

        private Boolean _wallCollisions = false;

        public CollisionControl(RobotSession robotSession)
        {
            _robotSession = robotSession;
        }

        private DeviceMessage CreateCollisionDeviceMessage(CollisionMethod method, Int32 xThreshold, Int32 xSpeedThreshold, Int32 yThreshold, Int32 ySpeedThreshold, Int32 quietTime)
        {
            Byte[] numArray = new Byte[] { (Byte)method, (Byte)xThreshold, (Byte)xSpeedThreshold, (Byte)yThreshold, (Byte)ySpeedThreshold, (Byte)quietTime };
            return new DeviceMessage(2, 18, numArray, DeviceMessage.SopParameters.ResetTimeout);
        }

        public void ReceiveCollisionData(CollisionData collisionData)
        {
            if ((collisionData == null ? false : _onCollisionDetectedEvent != null))
            {
                if (!_wallCollisions)
                {
                    _onCollisionDetectedEvent.Invoke(this, collisionData);
                }
                else if ((collisionData.CollisionPower.Y <= 100f || (Double)collisionData.CollisionAccelerometerReading.Y <= 0 ? (Double)(Math.Abs(collisionData.CollisionAccelerometerReading.X) + Math.Abs(collisionData.CollisionAccelerometerReading.X)) > 1 : true))
                {
                    _onCollisionDetectedEvent.Invoke(this, collisionData);
                }
            }
        }

        public void StartDetection(Int32 xThreshold, Int32 xSpeedThreshold, Int32 yThreshold, Int32 ySpeedThreshold, Int32 quietTime)
        {
            if ((xThreshold < 0 || xThreshold > 255 || yThreshold < 0 || yThreshold > 255 || xSpeedThreshold < 0 || xSpeedThreshold > 255 || xSpeedThreshold < 0 ? true : xSpeedThreshold > 255))
            {
                throw new ArgumentOutOfRangeException("Collision thresholds must be values 0-255 inclusive.");
            }
            if ((quietTime < 0 ? true : quietTime > 2550))
            {
                throw new ArgumentOutOfRangeException("Quiet time must be a value between 0-2550 inclusive. Specified in milliseconds");
            }
            _robotSession.WriteToRobot(CreateCollisionDeviceMessage(CollisionMethod.Default, xThreshold, xSpeedThreshold, yThreshold, ySpeedThreshold, quietTime / 10));
            _wallCollisions = false;
        }

        public void StartDetectionForWallCollisions()
        {
            _robotSession.WriteToRobot(CreateCollisionDeviceMessage(CollisionMethod.Default, 200, 0, 125, 0, 20));
            _wallCollisions = true;
        }

        public void StopDetection()
        {
            _robotSession.WriteToRobot(CreateCollisionDeviceMessage(CollisionMethod.Disabled, 0, 0, 0, 0, 0));
        }

        private event EventHandler<CollisionData> _onCollisionDetectedEvent;

        public event EventHandler<CollisionData> CollisionDetectedEvent
        {
            add
            {
                _onCollisionDetectedEvent = (EventHandler<CollisionData>)Delegate.Combine(_onCollisionDetectedEvent, value);
            }
            remove
            {
                _onCollisionDetectedEvent = (EventHandler<CollisionData>)Delegate.Remove(_onCollisionDetectedEvent, value);
            }
        }
    }
}