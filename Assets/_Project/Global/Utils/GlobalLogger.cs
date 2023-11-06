using Debug = UnityEngine.Debug;

namespace Game.Global.Management
{
    public static class GlobalLogger
    {
        static GlobalLogger()
        {
#if UNITY_EDITOR == false
            Debug.unityLogger.logEnabled = false;
#endif
        }

        public static void LogMessage(string message)
        {
            Debug.Log(message);
        }

        public static void LogError(string errorMessage)
        {
            Debug.LogError(errorMessage);
        }

        public static void LogWarning(string warningMessage)
        {
            Debug.LogWarning(warningMessage);
        }
    }
}