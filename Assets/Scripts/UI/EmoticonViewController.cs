using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Om
{
    public class EmoticonViewController : MonoBehaviour
    {
        public event UnityAction<Emoticon> OnClickEmoticon;

        [SerializeField] private GameObject _contentView;

        private bool _isDirty;

        private EmoticonGroupSO _emoticonGroupSO;
        private List<Emoticon> _emoticonList = new List<Emoticon>();

        public EmoticonGroupSO EmoticonGroupSO
        {
            get => _emoticonGroupSO;
            set
            {
                _emoticonGroupSO = value;
                _isDirty = true;
            }
        }

        private void OnEnable()
        {
            if (!_isDirty)
                return;

            foreach(var emoticon in _emoticonList)
            {
                emoticon.OnClick -= OnClick;
                Destroy(emoticon.gameObject);
            }

            _emoticonList.Clear();

            foreach (var emoticonPrefab in _emoticonGroupSO.Emoticons)
            {
                var emoticon = Instantiate(emoticonPrefab, _contentView.transform);
                emoticon.OnClick += OnClick;
                _emoticonList.Add(emoticon);
            }

            _isDirty = false;
        }

        private void OnClick(Emoticon emoticon)
        {
            OnClickEmoticon?.Invoke(emoticon);
        }
    }
}