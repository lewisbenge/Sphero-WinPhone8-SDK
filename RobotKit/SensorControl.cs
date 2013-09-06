using System;
using RobotKit.Internal;

namespace RobotKit
{
    public class SensorControl
    {
        private RobotSession _robotSession;

        private ulong _mask;

        private Int32 _hz = 20;

        private static Int32 _robotNativeHz;

        public Int32 Hz
        {
            get
            {
                return _hz;
            }
            set
            {
                _hz = value;
            }
        }

        public ulong Mask
        {
            get
            {
                return _mask;
            }
        }

        static SensorControl()
        {
            SensorControl._robotNativeHz = 400;
        }

        public SensorControl(RobotSession robotSession)
        {
            _robotSession = robotSession;
        }

        private DeviceMessage CreateDataStreamingMessage(Int32 hz, UInt64 mask)
        {
            Int32 num = 0;
            if (hz != 0)
            {
                num = SensorControl._robotNativeHz / hz;
            }
            Byte[] numArray = Value.BytesForInt(num);
            Byte[] numArray1 = Value.BytesForInt(1);
            Byte[] numArray2 = Value.BytesForLong((Int64)mask);
            Byte num1 = 0;
            Byte[] numArray3 = new Byte[] { numArray[0], numArray[1], numArray1[0], numArray1[1], numArray2[0], numArray2[1], numArray2[2], numArray2[3], num1 };
            return new DeviceMessage(2, 17, numArray3, DeviceMessage.SopParameters.ResetTimeout);
        }

        public void ReceiveSensorData(AsyncSensorData asyncData)
        {
            if ((asyncData.Attitude != null && _onAttitudeUpdatedEvent != null))
            {
                _onAttitudeUpdatedEvent.Invoke(this, asyncData.Attitude);
            }
            if ((asyncData.Accelorometer != null && _onAccelerometerUpdatedEvent != null))
            {
                _onAccelerometerUpdatedEvent.Invoke(this, asyncData.Accelorometer);
            }
            if ((asyncData.Gyrometer != null && _onGyrometerUpdatedEvent != null))
            {
                _onGyrometerUpdatedEvent.Invoke(this, asyncData.Gyrometer);
            }
            if ((asyncData.Quaternion != null && _onQuaternionUpdatedEvent != null))
            {
                _onQuaternionUpdatedEvent.Invoke(this, asyncData.Quaternion);
            }
            if ((asyncData.Location != null && _onLocationUpdatedEvent != null))
            {
                _onLocationUpdatedEvent.Invoke(this, asyncData.Location);
            }
            if ((asyncData.Velocity != null && _onVelocityUpdatedEvent != null))
            {
                _onVelocityUpdatedEvent.Invoke(this, asyncData.Velocity);
            }
        }

        public void StopAll()
        {
            _robotSession.WriteToRobot(CreateDataStreamingMessage(0, (UInt64)0));
        }

        private void UpdateMask()
        {
            _robotSession.WriteToRobot(CreateDataStreamingMessage(_hz, _mask));
        }

        private event EventHandler<AccelerometerReading> _onAccelerometerUpdatedEvent;

        private event EventHandler<AttitudeReading> _onAttitudeUpdatedEvent;

        private event EventHandler<GyrometerReading> _onGyrometerUpdatedEvent;

        private event EventHandler<LocationReading> _onLocationUpdatedEvent;

        private event EventHandler<QuaternionReading> _onQuaternionUpdatedEvent;

        private event EventHandler<VelocityReading> _onVelocityUpdatedEvent;

        public event EventHandler<AccelerometerReading> AccelerometerUpdatedEvent
        {
            add
            {
                _onAccelerometerUpdatedEvent = (EventHandler<AccelerometerReading>)Delegate.Combine(_onAccelerometerUpdatedEvent, value);
                _mask = _mask | (Int64)57344;
                UpdateMask();
            }
            remove
            {
                _onAccelerometerUpdatedEvent = (EventHandler<AccelerometerReading>)Delegate.Remove(_onAccelerometerUpdatedEvent, value);
            }
        }

        public event EventHandler<AttitudeReading> AttitudeUpdatedEvent
        {
            add
            {
                _onAttitudeUpdatedEvent = (EventHandler<AttitudeReading>)Delegate.Combine(_onAttitudeUpdatedEvent, value);
                _mask = _mask | (Int64)458752;
                UpdateMask();
            }
            remove
            {
                _onAttitudeUpdatedEvent = (EventHandler<AttitudeReading>)Delegate.Remove(_onAttitudeUpdatedEvent, value);
            }
        }

        public event EventHandler<GyrometerReading> GyrometerUpdatedEvent
        {
            add
            {
                _onGyrometerUpdatedEvent = (EventHandler<GyrometerReading>)Delegate.Combine(_onGyrometerUpdatedEvent, value);
                _mask = _mask | (Int64)7168;
                UpdateMask();
            }
            remove
            {
                _onGyrometerUpdatedEvent = (EventHandler<GyrometerReading>)Delegate.Remove(_onGyrometerUpdatedEvent, value);
            }
        }

        public event EventHandler<LocationReading> LocationUpdatedEvent
        {
            add
            {
                _onLocationUpdatedEvent = (EventHandler<LocationReading>)Delegate.Combine(_onLocationUpdatedEvent, value);
                _mask = _mask | 864691128455135232L;
                UpdateMask();
            }
            remove
            {
                _onLocationUpdatedEvent = (EventHandler<LocationReading>)Delegate.Remove(_onLocationUpdatedEvent, value);
            }
        }

        public event EventHandler<QuaternionReading> QuaternionUpdatedEvent
        {
            add
            {
                _onQuaternionUpdatedEvent = (EventHandler<QuaternionReading>)Delegate.Combine(_onQuaternionUpdatedEvent, value);
                _mask = _mask |  ulong.Parse("-1152921504606846976L");
                UpdateMask();
            }
            remove
            {
                _onQuaternionUpdatedEvent = (EventHandler<QuaternionReading>)Delegate.Remove(_onQuaternionUpdatedEvent, value);
            }
        }

        public event EventHandler<VelocityReading> VelocityUpdatedEvent
        {
            add
            {
                _onVelocityUpdatedEvent = (EventHandler<VelocityReading>)Delegate.Combine(_onVelocityUpdatedEvent, value);
                _mask = _mask | 108086391056891904L;
                UpdateMask();
            }
            remove
            {
                _onVelocityUpdatedEvent = (EventHandler<VelocityReading>)Delegate.Remove(_onVelocityUpdatedEvent, value);
            }
        }
    }
}