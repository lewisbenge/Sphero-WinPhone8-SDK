using System;
using System.Collections.Generic;

namespace RobotKit.Internal
{
    public class MetaResponse
    {
        private Byte _deviceId;

        private Byte _cmdId;

        private List<MetaField> _fields = new List<MetaField>();

        public MetaResponse(Byte deviceId, Byte cmdId)
        {
            _deviceId = deviceId;
            _cmdId = cmdId;
        }

        public void addField(MetaField f)
        {
            _fields.Add(f);
        }
    }
}