using System;
using System.Diagnostics;

namespace RobotKit.Internal
{
    public class DeviceMessage
    {
        private const Int32 kBasePackageLength = 7;

        private const Byte kSOP1 = 255;

        private const Int32 kDataStart = 6;

        private DeviceMessage.SopParameters m_flags;

        private Byte m_deviceId;

        private Byte m_commandId;

        private Byte _seqNum;

        private Byte[] m_data;

        private Byte[] m_package;

        private Boolean m_requestResponse = true;

        private Boolean m_mustGeneratePackage;

        public Byte[] Data
        {
            get
            {
                if (m_mustGeneratePackage)
                {
                    GeneratePackage();
                }
                Debug.WriteLine(String.Concat("OUT: ", DebugGoodies.ByteArrayToString(m_package)));
                return m_package;
            }
        }

        public DeviceMessage(Byte deviceId, Byte commandId, Byte[] data, SopParameters flags = SopParameters.Default)
        {
            m_flags = flags;
            m_deviceId = deviceId;
            m_commandId = commandId;
            m_data = data;
            m_mustGeneratePackage = true;
            m_requestResponse = true;
        }

        private void GeneratePackage()
        {
            Int32 length = (Int32)m_data.Length;
            m_package = new Byte[7 + length];
            m_package[0] = 255;
            if (!m_requestResponse)
            {
                m_package[1] = (Byte)(254 | (Int32)m_flags);
            }
            else
            {
                m_package[1] = (Byte)(255 | (Int32)m_flags);
            }
            m_package[2] = m_deviceId;
            m_package[3] = m_commandId;
            m_package[4] = _seqNum;
            m_package[5] = (Byte)(length + 1);
            Array.Copy(m_data, 0, m_package, 6, length);
            Byte mPackage = 0;
            for (Int32 i = 2; i < 6 + length; i++)
            {
                mPackage = (Byte)(mPackage + m_package[i]);
            }
            m_package[6 + (Int32)m_data.Length] = (Byte)(~mPackage);
            m_mustGeneratePackage = false;
        }

        public void NoResponse()
        {
            m_requestResponse = false;
        }

        internal void setSequence(Byte sequence)
        {
            _seqNum = sequence;
        }

        [Flags]
        public enum SopParameters
        {
            Answer = 1,
            Default = 2,
            ResetTimeout = 2
        }
    }
}