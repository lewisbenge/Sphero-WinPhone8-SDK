using System;

namespace RobotKit.Internal
{
    internal class DemoIdeas
    {
        public DemoIdeas()
        {
        }

        public void doThis()
        {
            CommandAndResponseSet commandAndResponseSet = new CommandAndResponseSet(0, 34);
            commandAndResponseSet.addParameter("Wakeup", 16);
            commandAndResponseSet.addParameter("MacroId", 8);
            commandAndResponseSet.addParameter("OrbBasicLineNum", 16);
        }

        public void potentialUsage()
        {
        }
    }
}