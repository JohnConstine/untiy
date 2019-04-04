#if !NOT_USE_UI_THREAD

using System;
using UnityEngine;

namespace SG1
{
    internal delegate void ThreadDispatchDelegate();

    internal delegate void ThreadDispatchDelegateArg(object arg);

    internal class ThreadDispatchAction
    {
        private object _dispatchArgParam;
        private ThreadDispatchDelegateArg _dispatchCallArg;
        private ThreadDispatchDelegate _dispatchCallClean;
        private bool _safeMode;
        public bool Executed;

        public void ExecuteDispatch()
        {
            if (_safeMode)
            {
                try
                {
                    if (_dispatchCallClean != null)
                        _dispatchCallClean();
                    else
                    {
                        _dispatchCallArg?.Invoke(_dispatchArgParam);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
            {
                if (_dispatchCallClean != null)
                    _dispatchCallClean();
                else
                {
                    _dispatchCallArg?.Invoke(_dispatchArgParam);
                }
            }

            Executed = true;
        }

        public void Init(ThreadDispatchDelegate dispatchCall, bool safeMode)
        {
            _safeMode = safeMode;
            _dispatchCallClean = dispatchCall;
        }

        public void Init(ThreadDispatchDelegateArg dispatchCall, object dispatchArgumentParameter, bool safeMode)
        {
            _safeMode = safeMode;
            _dispatchCallArg = dispatchCall;
            _dispatchArgParam = dispatchArgumentParameter;
        }
    }
}

#endif