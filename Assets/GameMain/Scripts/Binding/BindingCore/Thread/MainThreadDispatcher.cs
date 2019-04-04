#if !NOT_USE_UI_THREAD

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SG1
{
    internal static class MainThreadDispatcher
    {
        private static readonly List<ThreadDispatchAction> DispatchActions = new List<ThreadDispatchAction>();
        public static GameObject GameObject { get; set; }

        public static IEnumerator DispatcherCoroutine()
        {
            yield return null;
            while (true)
            {
                if (DispatchActions != null && DispatchActions.Count > 0)
                    lock (DispatchActions)
                    {
                        foreach (var action in DispatchActions)
                            if (!action.Executed)
                                action.ExecuteDispatch();
                        DispatchActions.Clear();
                        Monitor.PulseAll(DispatchActions);
                    }

                yield return null;
            }
        }

        internal static void DispatchToMainThread(ThreadDispatchDelegate dispatchCall, bool safeMode = true)
        {
            if (MainThreadDispatchHelper.CheckIfMainThread())
            {
                dispatchCall?.Invoke();
            }
            else
            {
                var action = new ThreadDispatchAction();
                lock (DispatchActions)
                {
                    DispatchActions.Add(action);
                }

                action.Init(dispatchCall, safeMode);
            }
        }

        internal static void DispatchToMainThread(ThreadDispatchDelegateArg dispatchCall, object dispatchArgument,
            bool safeMode = true)
        {
            if (MainThreadDispatchHelper.CheckIfMainThread())
            {
                dispatchCall?.Invoke(dispatchArgument);
            }
            else
            {
                var action = new ThreadDispatchAction();
                lock (DispatchActions)
                {
                    DispatchActions.Add(action);
                }

                action.Init(dispatchCall, dispatchArgument, safeMode);
            }
        }
    }
}

#endif