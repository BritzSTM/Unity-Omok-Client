using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Om
{
    public class EmoticonGroupViewController : MonoBehaviour
    {
        public event UnityAction<EmoticonGroupSO> OnClickEmoticonGroup;

        [SerializeField] private GameObject _contentView;
        [SerializeField] private EmoticonGroupSO[] _emoticonGroupSOs;

        private Dictionary<string, EmoticonGroupSO> _groupDic;
        private void Awake()
        {
            _groupDic = new Dictionary<string, EmoticonGroupSO>();

            foreach(var groupSO in _emoticonGroupSOs)
            {
                var groupCover = Instantiate(groupSO.Emoticons[0], _contentView.transform);
                groupCover.OnClick += OnClickGroup;
                _groupDic[groupCover.DisplayName] = groupSO;
            }
        }

        private void OnClickGroup(Emoticon emoticon)
        {
            OnClickEmoticonGroup?.Invoke(_groupDic[emoticon.DisplayName]);
        }
    }
}