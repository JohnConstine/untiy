using System.Collections.Generic;
using System.Net;
using ExitGames.Client.Photon;
using GameFramework.Event;
using IPhotonChannel = GameFramework.Photon.IPhotonChannel;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class TestPageModel : UGuiFormModel<TestPage, TestPageModel>
    {
        public void OnTapConnect()
        {
            Page.Connect();
        }

        public void OnTapSendMessage()
        {
            Page.SendMessage();
        }
    }

//    public class TestPage : UGuiFormPage<TestPage, TestPageModel>, IPhotonPeerListener
//    {
//        PhotonPeer peer;
//
//        // Start is called before the first frame update
//        protected override void OnOpen(object userData)
//        {
//            base.OnOpen(userData);
//        }
//
//        public void Connect()
//        {
//            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
//            peer.Connect("192.168.1.249:5055", "SGServer");
//            peer.Service();
//        }
//
//        // Update is called once per frame
//        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
//        {
//            base.OnUpdate(elapseSeconds, realElapseSeconds);
//            if (peer != null)
//            {
//                peer.Service();
//            }
//        }
//
//        public void SendMessage()
//        {
//            if (peer.PeerState == PeerStateValue.Connected)
//            {
//                Dictionary<byte, object> data = new Dictionary<byte, object>();
//                data.Add(0, 1);
//                data.Add(1, "abc");
//                peer.OpCustom(4, data, true); //1:请求 2：事件
//            }
//        }
//
//        //当游戏关闭的时候（停止运行）调用OnDestroy
//        protected override void OnClose(object userData)
//        {
//            //如果peer不等于空并且状态为正在连接
//            if (peer != null && peer.PeerState == PeerStateValue.Connected)
//            {
//                peer.Disconnect(); //断开连接
//            }
//
//            base.OnClose(userData);
//        }
//
//        public void DebugReturn(DebugLevel level, string message)
//        {
//        }
//
//        //如果客户端没有发起请求，但是服务器端向客户端通知一些事情的时候就会通过OnEvent来进行响应 
//        public void OnEvent(EventData eventData)
//        {
//            Debug.Log(eventData.Code);
//            switch (eventData.Code)
//            {
//                case 0:
//                    Log.Info("收到服务器发过来的事件 :" + eventData.Code);
//                    Dictionary<byte, object> data3 = eventData.Parameters;
//                    object intvalue;
//                    data3.TryGetValue(1, out intvalue);
//                    object stringValue;
//                    data3.TryGetValue(2, out stringValue);
//                    Log.Info(intvalue.ToString() + "  " + stringValue.ToString());
//                    break;
//                default:
//                    Log.Info("收到服务器发过来的事件 :" + eventData.Code);
//                    Dictionary<byte, object> data4 = eventData.Parameters;
//                    Log.Info(data4.Count);
//                    object intvalue4;
//                    data4.TryGetValue(1, out intvalue4);
//                    Log.Info(intvalue4.ToString() + "  ");
//                    break;
//            }
//        }
//
//        //当我们在客户端向服务器端发起请求后，服务器端接受处理这个请求给客户端一个响应就会在这个方法里进行处理
//        public void OnOperationResponse(OperationResponse operationResponse)
//        {
//            Debug.Log(operationResponse.ToStringFull());
//            switch (operationResponse.OperationCode)
//            {
//                case 1:
//                    Log.Info("收到了服务器端的响应");
//                    //接受服务器的数据
//                    Dictionary<byte, object> data = operationResponse.Parameters;
//                    object intValue;
//                    object StringValue;
//                    data.TryGetValue(1, out intValue);
//                    data.TryGetValue(2, out StringValue);
//                    // Log.Info("数据信息:" + intValue.ToString() + StringValue.ToString());
//                    break;
//                default:
//                    break;
//            }
//        }
//
//        //如果连接状态发生改变的时候就会触发这个方法。
//        //连接状态有五种，正在连接中(PeerStateValue.Connecting)，已经连接上（PeerStateValue.Connected），正在断开连接中( PeerStateValue.Disconnecting)，已经断开连接(PeerStateValue.Disconnected)，正在进行初始化(PeerStateValue.InitializingApplication)
//        public void OnStatusChanged(StatusCode statusCode)
//        {
//            Log.Info(statusCode + "^^^^");
//        }
//    }
//}

    public class TestPage : UGuiFormPage<TestPage, TestPageModel>
    {
        private IPhotonChannel m_PhotonChannel;

        [SerializeField] private string m_IP = "127.0.0.1";
        [SerializeField] private int m_Port = 5055;

        public void Connect()
        {
            m_PhotonChannel.Connect(IPAddress.Parse(m_IP), m_Port, ConnectionProtocol.Udp);
        }

        public void SendMessage()
        {
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(0, 1);
            data.Add(1, "abc");
            m_PhotonChannel.SendRequset(4, data, true); //1:请求 2：事件
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_PhotonChannel =
                GameEntry.Photon.HasPhotonChannel("SGServer")
                    ? GameEntry.Photon.GetPhotonChannel("SGServer")
                    : GameEntry.Photon.CreatePhotonChannel("SGServer");

            GameEntry.Event.Subscribe(PhotonEventEventArgs.EventId, OnPhotonEventEvent);
            GameEntry.Event.Subscribe(PhotonOperationResponseEventArgs.EventId, OnPhotonOperationResponse);
            GameEntry.Event.Subscribe(PhotonStatusChangedEventArgs.EventId, OnPhotonStatusChanged);
        }

        private void OnPhotonEventEvent(object sender, GameEventArgs e)
        {
            PhotonEventEventArgs ne = e as PhotonEventEventArgs;
            if (ne.PhotonChannel != m_PhotonChannel)
            {
                return;
            }

            var eventData = ne.EventData;

            switch (eventData.Code)
            {
                case 0:
                    Log.Info("收到服务器发过来的事件 :" + eventData.Code);
                    Dictionary<byte, object> data3 = eventData.Parameters;
                    object intvalue;
                    data3.TryGetValue(1, out intvalue);
                    object stringValue;
                    data3.TryGetValue(2, out stringValue);
                    Debug.Log(intvalue.ToString() + "  " + stringValue.ToString());
                    break;
                default:
                    Log.Info("收到服务器发过来的事件 :" + eventData.Code);
                    Dictionary<byte, object> data4 = eventData.Parameters;
                    Debug.Log(data4.Count);
                    object intvalue4;
                    data4.TryGetValue(1, out intvalue4);
                    Debug.Log(intvalue4.ToString() + "  ");
                    break;
            }
        }

        private void OnPhotonOperationResponse(object sender, GameEventArgs e)
        {
            PhotonOperationResponseEventArgs ne = e as PhotonOperationResponseEventArgs;
            if (ne.PhotonChannel != m_PhotonChannel)
            {
                return;
            }

            var operationResponse = ne.OperationResponse;

            Debug.Log(operationResponse.ToStringFull());
            switch (operationResponse.OperationCode)
            {
                case 1:
                    Log.Info("收到了服务器端的响应");
                    //接受服务器的数据
                    Dictionary<byte, object> data = operationResponse.Parameters;
                    object intValue;
                    object StringValue;
                    data.TryGetValue(1, out intValue);
                    data.TryGetValue(2, out StringValue);
                    // Debug.Log("数据信息:" + intValue.ToString() + StringValue.ToString());
                    break;
                default:
                    break;
            }
        }

        private void OnPhotonStatusChanged(object sender, GameEventArgs e)
        {
            PhotonStatusChangedEventArgs ne = e as PhotonStatusChangedEventArgs;
            if (ne.PhotonChannel != m_PhotonChannel)
            {
                return;
            }
            
            Log.Info(ne.StatusCode);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(PhotonEventEventArgs.EventId, OnPhotonEventEvent);
            GameEntry.Event.Unsubscribe(PhotonOperationResponseEventArgs.EventId, OnPhotonOperationResponse);
            GameEntry.Event.Unsubscribe(PhotonStatusChangedEventArgs.EventId, OnPhotonStatusChanged);

            base.OnClose(userData);
        }
    }
}