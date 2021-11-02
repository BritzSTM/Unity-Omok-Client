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



    public class NetService : MonoBehaviour
    {
        [SerializeField] private string _serverUrl;
        [SerializeField] private GameObject _connectView;
        [SerializeField] private GameObject _contentView;
        [SerializeField] private GameObject _roomView;
        [SerializeField] private TMPro.TMP_InputField _nickNameField;
        [SerializeField] private PlayerListViewController _playerListViewController;
        [SerializeField] private ChatViewContoller _chatViewContoller;

        private Socket _sock;
        public void Connect(string nickName)
        {
            _sock = Socket.Connect(_serverUrl);

            _sock.On(SystemEvents.connect, () => {
                Debug.Log("Connect");
                _sock.Emit("JoinChannel", _nickNameField.text);
            });

            _sock.On("ChatTo", ChatTo);
            _sock.On("JoinedChannel", JoinedChannel);
            _sock.On("JoinedNewPlayer", JoinedNewPlayer);
        }

        private void JoinedChannel(string data)
        {
            Debug.Log("JoinedChannel");
            _connectView.SetActive(false);
            _roomView.SetActive(true);

            var channelInfo = JsonConvert.DeserializeObject<ChannelInfo>(data);
            foreach (var nick in channelInfo.Users)
            {
                _playerListViewController.AddPlayer(nick);
            }

            Debug.Log("Joined Channel name : " + channelInfo.Name);
        }

        private void JoinedNewPlayer(string data)
        {
            string nick = JsonConvert.DeserializeObject<string>(data);
            _playerListViewController.AddPlayer(nick);
        }

        public void SendChat(ChatMessageData message)
        { 
            string data = JsonConvert.SerializeObject(message);
            _sock.EmitJson("Chat", data);

            Debug.Log("Send Chat : " + data);
        }

        private void ChatTo(string data)
        {
            var chatData = JsonConvert.DeserializeObject<ChatMessageData>(data);
            _chatViewContoller.Speek(ChatWhoType.OtherPlayer, chatData);
        }

        private void OnDestroy()
        {
            _sock.Emit("LeaveChannel");
        }
    }
}