using MelonLoader;

namespace NastyMod_v2.Core
{
    /**
     * Helper
     * 
     * This is the helper class for the NastyMod project.
     * 
     * Author: nastycodes
     * Version: 1.0.0
     */
    public class Helper
    {
        /**
         * SendLoggerMsg
         * 
         * This method sends a message to the logger.
         * 
         * @param message The message to send.
         * @param type The type of message to send. (Msg, Error, Warning)
         * @return void
         */
        public void SendLoggerMsg(string message, string type = "")
        {
            switch (type)
            {
                case "msg":
                    MelonLogger.Msg(message);
                    break;
                case "error":
                    MelonLogger.Error(message);
                    break;
                case "warning":
                    MelonLogger.Warning(message);
                    break;
                default:
                    MelonLogger.Msg(message);
                    break;
            }
        }
    }
}
