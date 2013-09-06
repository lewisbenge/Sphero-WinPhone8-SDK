using System;

namespace RobotKit.Internal
{
    internal class ExecutableCommand
    {
        private Byte deviceId;

        private Byte cmdId;

        private Boolean responseRequested = true;

        public ExecutableCommand()
        {
        }
    }
}