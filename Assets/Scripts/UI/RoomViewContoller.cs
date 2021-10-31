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

        private void Awake()
        {
            _inputField.onSubmit.AddListener(SendChat);
        }

        private void SendChat(string text)
        {
            ChatMessage msg;
            msg.MessageType = ChatMessage.Type.Message;
            msg.Data = text;

            @NetService.SendChat(msg);
        }

        private void SendEmoticon()
        {

        }
    }
}