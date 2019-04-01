using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace SG1
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/XLua")]
    public sealed class XLuaComponent : GameFrameworkComponent
    {
        private XLuaWindow m_XLuaWindow = new XLuaWindow();

        private LoadAssetCallbacks m_LoadLuaFileCallbacks;

        private IResourceManager m_ResourceManager;

        private LuaEnv m_LuaEvn;

        private LuaTable m_ScriptTable;

        private Dictionary<string, LuaFileInfo> m_CacheLuaDict;

        [SerializeField] private float m_GCInterval = 1; // 1 秒

        [SerializeField] private float m_LastGCTime = 0;

        public LuaTable ScriptTable
        {
            get { return m_ScriptTable; }
        }

        public float GCInterval
        {
            get { return m_GCInterval; }
            set { m_GCInterval = value; }
        }

        public float LastGCTime
        {
            get { return m_LastGCTime; }
            set { m_LastGCTime = value; }
        }

        protected override void Awake()
        {
            m_LoadLuaFileCallbacks = new LoadAssetCallbacks(LoadLuaSuccessCallback, LoadLuaFailureCallback);

            m_CacheLuaDict = new Dictionary<string, LuaFileInfo>();

            // 设置Lua环境
            m_LuaEvn = new LuaEnv();
            m_ScriptTable = m_LuaEvn.NewTable();
            LuaTable meta = m_LuaEvn.NewTable();
            meta.Set("__index", m_LuaEvn.Global);
            m_ScriptTable.SetMetaTable(meta);
            meta.Dispose();

            // 设置Lua self
            m_ScriptTable.Set("self", this);

            // 设置自定义Loader
            m_LuaEvn.AddLoader(CustomLoader);

            base.Awake();
        }

        private IEnumerator Start()
        {
            BaseComponent baseComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                yield break;
            }

            DebuggerComponent debuggerComponent =
                UnityGameFramework.Runtime.GameEntry.GetComponent<DebuggerComponent>();
            if (debuggerComponent == null)
            {
                Log.Fatal("Debugger component is invalid.");
                yield break;
            }

            if (baseComponent.EditorResourceMode)
            {
                this.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                this.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }
            
            yield return new WaitForEndOfFrame();
            debuggerComponent.RegisterDebuggerWindow("XLua", m_XLuaWindow);
        }

        public void Initializtion()
        {
            // 这个初始化更新文件
            LoadLua(Constant.XLua.HotfixFileName, LoadType.Text, luaFileInfo => DoLuaFile(luaFileInfo));
        }

        public void LoadLua(string luaName, LoadType loadType, OnLoadLuaFile onLoadAction)
        {
            LuaFileInfo luaFileInfo = new LuaFileInfo(luaName, onLoadAction);
            m_ResourceManager.LoadAsset(AssetUtility.GetLuaAsset(luaName, LoadType.Text),
                Constant.AssetPriority.LuaAsset, this.m_LoadLuaFileCallbacks, luaFileInfo);
        }

        public void DoLuaFile(LuaFileInfo luaFileInfo)
        {
            DoString(luaFileInfo.Bytes, luaFileInfo.LuaName, m_ScriptTable);
        }

        public void DoString(string chunk, string chunkName = "chunk", LuaTable env = null)
        {
            m_LuaEvn.DoString(chunk, chunkName, env);
        }

        public void DoString(byte[] chunk, string chunkName = "chunk", LuaTable env = null)
        {
            m_LuaEvn.DoString(chunk, chunkName, env);
        }

        private void Update()
        {
            if (Time.time - m_LastGCTime > m_GCInterval)
            {
                m_LuaEvn.Tick();
                m_LastGCTime = Time.time;
            }
        }

        private void OnDestroy()
        {
            m_LuaEvn.Dispose();
            m_CacheLuaDict.Clear();
            m_ScriptTable = null;
            m_LuaEvn = null;
        }

        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
                throw new GameFrameworkException("Resource manager is invalid.");
            m_ResourceManager = resourceManager;
        }

        private byte[] CustomLoader(ref string path)
        {
            if (m_CacheLuaDict.TryGetValue(path, out LuaFileInfo luaFileInfo))
            {
                return luaFileInfo.Bytes;
            }
            else
            {
                return (byte[]) null;
            }
        }

        private void LoadLuaSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            LuaFileInfo luaFileInfo = (LuaFileInfo) userData;

            if (luaFileInfo == null)
                throw new GameFrameworkException("Load lua file info is invalid.");

            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                Log.Warning("lua asset '{0}' is invalid.", assetName);
                return;
            }

            luaFileInfo.Bytes = textAsset.bytes;

            if (!m_CacheLuaDict.ContainsKey(luaFileInfo.LuaName))
            {
                m_CacheLuaDict.Add(luaFileInfo.LuaName, luaFileInfo);
                Log.Info("Load lua file '{0}' OK.", luaFileInfo.LuaName);
            }
            else
            {
                m_CacheLuaDict[luaFileInfo.LuaName] = luaFileInfo;
                Log.Warning("Already exist lua file '{0}'.", luaFileInfo.LuaName);
            }

            if (luaFileInfo.OnLoadSuccess != null)
            {
                luaFileInfo.OnLoadSuccess.Invoke(luaFileInfo);
            }

            luaFileInfo.ClearLuaReference();
        }

        private void LoadLuaFailureCallback(string assetName, LoadResourceStatus status, string errorMessage,
            object userData)
        {
            LuaFileInfo luaFileInfo = (LuaFileInfo) userData;

            if (luaFileInfo == null)
                throw new GameFrameworkException("Load lua file info is invalid.");
            //TODO:处理载入错误
            throw new GameFrameworkException(Utility.Text.Format(
                "Load lua file failure, asset name '{0}', status '{1}', error message '{2}'.", (object) assetName,
                (object) status.ToString(), (object) errorMessage));
        }
    }
}