using System;
using GameFramework;

namespace SG1
{
    public delegate void OnLoadLuaFile(LuaFileInfo luaFileInfo);
    public sealed class LuaFileInfo
    {
        private string m_LuaName = string.Empty;
        private byte[] m_Bytes;
        private OnLoadLuaFile m_OnLoadSuccess;

        public LuaFileInfo(string luaName,OnLoadLuaFile onLoadSuccess)
        {
            m_LuaName = luaName;
            m_OnLoadSuccess = onLoadSuccess;
        }

        public string LuaName
        {
            get
            {
                return m_LuaName;
            }
        }

        public byte[] Bytes
        {
            get
            {
                return m_Bytes;
            }
            set
            {
                m_Bytes = value;
            }
        }

        public OnLoadLuaFile OnLoadSuccess
        {
            get
            {
                return m_OnLoadSuccess;
            }
        }

        public void ClearLuaReference()
        {
            m_OnLoadSuccess = null;
        }
    }
}