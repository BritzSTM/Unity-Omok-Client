using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using socket.io;

namespace Om
{
    [System.Serializable]
    public struct ChannelInfo
    {
        public string Name;
        public string[] Users;
    }

    [System.Serializable]
    public struct ChatMessage
    {
        public enum Type
        {
            Emoticon,
            Message
        };

        public Type MessageType;
        public string Data;
    }

    public class NetService : MonoBehaviour
    {
        [SerializeField] private string _serverUrl;
        [SerializeField] private GameObject _connectView;
        [SerializeField] private GameObject _contentView;
        [SerializeField] private GameObject _roomView;

        private Socket _sock;
        public void Connect(string nickName)
        {
            _sock = Socket.Connect(_serverUrl);

            _sock.On(SystemEvents.connect, () => { 
                Debug.Log("Connect");
                //_sock.Emit("Chat", "ABCDEFG");
            });

            _sock.On("ChatTo", (string s) => Debug.Log(s));
            _sock.On("JoinedChannel", JoinedChannel);
        }

        private void JoinedChannel(string data)
        {
            Debug.Log("JoinedChannel");
            _connectView.SetActive(false);
            _roomView.SetActive(true);

            var channelInfo = JsonConvert.DeserializeObject<ChannelInfo>(data);
            Debug.Log(channelInfo);
        }

        public void SendChat(ChatMessage message)
        { 
            string data = JsonConvert.SerializeObject(message);
            _sock.EmitJson("Chat", data);

            Debug.Log("Send Chat : " + data);
        }
    }
}