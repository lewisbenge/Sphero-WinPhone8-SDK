using System;
using System.Collections.Generic;

namespace RobotKit.Internal
{
    internal class CommandAndResponseSet
    {
        private Byte _deviceId;

        private Byte _cmdId;

        private Boolean _requestResponse = true;

        private Single minFirmwareVersion = 0f;

        private Single maxFirmwareVersion = 2f;

        private List<CommandParam> parameters = new List<CommandParam>();

        public CommandAndResponseSet(Byte deviceId, Byte cmdId)
        {
            _deviceId = deviceId;
            _cmdId = cmdId;
        }

        internal void addParameter(String parameterName, Int32 length)
        {
        }


    }
}