using System.Linq;
using System.Text;

namespace RobotKit.Internal
{
    public enum AsynchronousId
    {
        POWER_NOTIFICATION = 1,
        LEVEL_1_DIAGNOSTIC = 2,
        SENSOR_DATA_STREAMING = 3,
        CONFIG_BLOCK_CONTENTS = 4,
        PRE_SLEEP_WARNING = 5,
        MACRO_MARKERS = 6,
        COLLISION_DETECTED = 7,
        ORBBASIC_PRINT_MSG = 8,
        ORBBASIC_ERROR_MSG_ASCII = 9,
        ORBBASIC_ERROR_MSG_BINARY = 10,
        SELF_LEVEL_RESULT = 11
    }
}
