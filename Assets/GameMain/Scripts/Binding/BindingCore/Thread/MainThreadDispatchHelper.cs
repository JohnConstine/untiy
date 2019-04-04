#if !NOT_USE_UI_THREAD

using System.Threading;
using UnityEngine;

namespace SG1
{
    internal class MainThreadDispatchHelper : MonoBehaviour
    {
        private static Thread _mainThread;

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            _mainThread = Thread.CurrentThread;
            var go = new GameObject("MainThreadDispatchHelper", typeof(MainThreadDispatchHelper))
            {
                hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector
            };
            DontDestroyOnLoad(go);
            var dispatchHelper = go.GetComponent<MainThreadDispatchHelper>();
            MainThreadDispatcher.GameObject = go;
            dispatchHelper.StartCoroutine(MainThreadDispatcher.DispatcherCoroutine());
        }

        public static bool CheckIfMainThread()
        {
            return Thread.CurrentThread == _mainThread;
        }
    }
}

#endif