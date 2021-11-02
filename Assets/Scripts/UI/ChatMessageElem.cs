using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Om
{ 
    public class ChatMessageElem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickText;
        [SerializeField] private TMP_Text _messageText;

        private RectTransform _rectTransform;
        private Image _image;
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            
        }

        public void Assign(ChatMessageData messageData)
        {
            if (!string.IsNullOrEmpty(messageData.NickName))
                _nickText.text = messageData.NickName;
            else
                _nickText.gameObject.SetActive(false);

            if (messageData.MessageType == ChatMessageData.Type.Message)
                _messageText.text = messageData.Data;
            else
                _messageText.gameObject.SetActive(false);
        }

        public void ChangeColor(ref Color color) => _image.color = color;
    }
}