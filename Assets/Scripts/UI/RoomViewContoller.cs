using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Om
{
    public class RoomViewContoller : MonoBehaviour
    {
        public NetService @NetService;
        public TMP_InputField _inputField;
        public ChatViewContoller _chatViewContoller;
        public EmoticonSelectViewController _emoticonSelectViewController;

        private void Awake()
        {
            _inputField.onSubmit.AddListener(SendChat);
            _emoticonSelectViewController.OnClickEmoticon += SendEmoticon;
        }

        private void Start()
        {
            
        }

        private ChatMessageData _msg = new ChatMessageData();
        private void SendChat(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                return;

            _msg.MessageType = ChatMessageData.Type.Message;
            _msg.Data = text;
            _chatViewContoller.Speek(ChatWhoType.Self, _msg);

            @NetService.SendChat(_msg);
            _inputField.text = "";
        }

        private void SendEmoticon(Emoticon emoticon)
        {
            _msg.MessageType = ChatMessageData.Type.Emoticon;
            _msg.Data = emoticon.DisplayName;
            _chatViewContoller.Speek(ChatWhoType.Self, _msg);

            @NetService.SendChat(_msg);
            _inputField.text = "";
        }
    }
}