using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alec.Utils
{
    class Debugger
    {
#if !Development
        public static void Log(object log,params object[] parameters)
        {
            UnityEngine.Debug.Log(string.Format(log.ToString(),parameters));
        }

        public static void LogWarning(object log, params object[] parameters)
        {
            UnityEngine.Debug.LogWarning(string.Format(log.ToString(), parameters));
        }

        public static void LogError(object log, params object[] parameters)
        {
            UnityEngine.Debug.LogError(string.Format(log.ToString(), parameters));
        }
        
#endif

    }
}
