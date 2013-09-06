namespace RobotKit.Internal
{
    public enum ResponseCode
    {
        OK = 0,
        GENERAL_ERROR = 1,
        CHECKSUM_ERROR = 2,
        FRAGMENT_ERROR = 3,
        BAD_COMMAND_ERROR = 4,
        UNSUPPORTED_ERROR = 5,
        BAD_MESSAGE_ERROR = 6,
        PARAMETER_ERROR = 7,
        EXECUTE_ERROR = 8,
        UNKNOWN_DEVICE_ID = 9,
        LOW_VOLTAGE_ERROR = 49,
        ILLEGAL_PAGE_NUMBER = 50,
        FLASH_FAIL = 51,
        MAIN_APP_CORRUPT = 52,
        RESPONSE_TIMEOUT = 53,
        TIMEOUT_ERROR = 255
    }
}