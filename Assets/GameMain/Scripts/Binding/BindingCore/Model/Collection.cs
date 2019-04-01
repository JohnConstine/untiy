using System;
using UnityEngine.Experimental.XR;

namespace SG1
{
    public abstract class Collection : EventArgs
    {
        public abstract bool HasItems { get; }

        public abstract int Count { get; }
    }
}