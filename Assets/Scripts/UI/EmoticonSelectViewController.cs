using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Om
{
    public class EmoticonSelectViewController : MonoBehaviour
    {
        public event UnityAction<Emoticon> OnClickEmoticon;

        [SerializeField] private Button _backButton;
        [SerializeField] private EmoticonGroupViewController _groupView;
        [SerializeField] private EmoticonViewController _emoticonView;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnClickBackButton);
            _groupView.OnClickEmoticonGroup += OnClickGroup;
            _emoticonView.OnClickEmoticon += OnClickEmoticonEvent;

            _backButton.gameObject.SetActive(false);
            _groupView.gameObject.SetActive(true);
            _emoticonView.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnClickBackButton);
            _groupView.OnClickEmoticonGroup -= OnClickGroup;
            _emoticonView.OnClickEmoticon -= OnClickEmoticonEvent;
        }

        private void OnClickGroup(EmoticonGroupSO groupSO)
        {
            _emoticonView.EmoticonGroupSO = groupSO;

            _backButton.gameObject.SetActive(true);
            _groupView.gameObject.SetActive(false);
            _emoticonView.gameObject.SetActive(true);
        }

        private void OnClickEmoticonEvent(Emoticon emoticon)
        {
            Debug.Log("Selected : " + emoticon.DisplayName);
            OnClickEmoticon?.Invoke(emoticon);
            gameObject.SetActive(false);
        }

        private void OnClickBackButton()
        {
            _backButton.gameObject.SetActive(false);
            _groupView.gameObject.SetActive(true);
            _emoticonView.gameObject.SetActive(false);
        }
    }
}