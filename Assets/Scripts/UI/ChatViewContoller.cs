using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Om
{
    [System.Serializable]
    public struct ChatMessageData
    {
        public enum Type
        {
            Message,
            Emoticon
        };

        public Type MessageType;
        public string NickName;
        public string Data;
    }

    public enum ChatWhoType
    {
        None,
        Self,
        OtherPlayer
    }

    public class ChatViewContoller : MonoBehaviour
    {
        [SerializeField] private int _chatMaxCount = 10;
        [SerializeField] private GameObject _contentView;
        private ContentSizeFitter _fitter;

        [SerializeField] private GameObject _chatMessageElemPrefab;
        [SerializeField] private EmoticonGroupSO[] _emoticonGroupSOs;

        private List<ChatMessageElem> _chatElems = new List<ChatMessageElem>();

        public void Speek(ChatWhoType who, ChatMessageData message)
        {
            var newChatElem = Instantiate(_chatMessageElemPrefab, _contentView.transform);
            var chatElem = newChatElem.GetComponent<ChatMessageElem>();
            chatElem.Assign(message);

            if (who == ChatWhoType.Self)
            {
                Color color;
                ColorUtility.TryParseHtmlString("#FFCC1FFF", out color); // yellow
                chatElem.ChangeColor(ref color);

            }

            if (message.MessageType == ChatMessageData.Type.Emoticon)
            {
                foreach(var group in _emoticonGroupSOs)
                {
                    foreach(var emotion in group.Emoticons)
                    {
                        if(emotion.DisplayName == message.Data)
                        {
                            var emticonInst = Instantiate(emotion, chatElem.transform);
                            goto FINISH;
                        }
                    }
                }

                Debug.Log("Not found emoticon : " + message.Data);
            }
        FINISH:
            _chatElems.Add(chatElem);
            if (_chatElems.Count > _chatMaxCount)
            {
                var oldChatElem = _chatElems[0];
                _chatElems.RemoveAt(0);

                Destroy(oldChatElem.gameObject);
            }

            if (_fitter == null)
                _fitter = _contentView.GetComponent<ContentSizeFitter>();

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_fitter.transform);
        }
    }
}