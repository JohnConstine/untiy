using System;
using System.Collections.Generic;
using XLua;
using System.Linq;
using System.Reflection;
using GameFramework.UI;
using UnityEngine.Experimental.XR;
using UnityGameFramework.Runtime;

namespace SG1.Editor
{
    public static class XLuaConfig
    {
        [LuaCallCSharp]
        public static IEnumerable<Type> LuaCallCSharp
        {
            get
            {
                return from type in Assembly.Load("Assembly-CSharp").GetTypes()
                    where type.BaseType == typeof(UIForm) || type.FullName.EndsWith("Extension")
                    select type;
            }
        }

        [LuaCallCSharp]
        public static IEnumerable<Type> LuaCallCSharp_UnityGameFramework
        {
            get
            {
                return from type in Assembly.Load("UnityGameFramework.Runtime").GetTypes()
                    where type.BaseType == typeof(GameFrameworkComponent)
                    select type;
            }
        }

        //黑名单
        [BlackList] public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>() {"System.Xml.XmlNodeList", "ItemOf"},
            new List<string>() {"UnityEngine.WWW", "movie"},
#if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
            new List<string>() {"UnityEngine.Texture2D", "alphaIsTransparency"},
            new List<string>() {"UnityEngine.Security", "GetChainOfTrustValue"},
            new List<string>() {"UnityEngine.CanvasRenderer", "onRequestRebuild"},
            new List<string>() {"UnityEngine.Light", "areaSize"},
            new List<string>() {"UnityEngine.Light", "lightmapBakeType"},
            new List<string>() {"UnityEngine.WWW", "MovieTexture"},
            new List<string>() {"UnityEngine.WWW", "GetMovieTexture"},
            new List<string>() {"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
#if !UNITY_WEBPLAYER
            new List<string>() {"UnityEngine.Application", "ExternalEval"},
#endif
            new List<string>() {"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
            new List<string>() {"UnityEngine.Component", "networkView"}, //4.6.2 not support
            new List<string>()
                {"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>() {"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
            new List<string>()
                {"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
            new List<string>()
                {"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>()
            {
                "System.IO.DirectoryInfo", "CreateSubdirectory", "System.String",
                "System.Security.AccessControl.DirectorySecurity"
            },
            new List<string>() {"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
            new List<string>() {"UnityEngine.MonoBehaviour", "runInEditMode"},
        };
    }
}