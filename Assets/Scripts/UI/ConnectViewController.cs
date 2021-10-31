using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Om
{
    public class ConnectViewController : MonoBehaviour
    {
        public NetService NetService;

        [SerializeField] private TMP_InputField _nickNameField;
        [SerializeField] private Button _connectButton;

        private void Awake()
        {
            _connectButton.onClick.AddListener(() => { NetService.Connect(_nickNameField.text); });
        }
    }
}