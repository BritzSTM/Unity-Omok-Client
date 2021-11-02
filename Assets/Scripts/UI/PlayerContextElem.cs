using UnityEngine;
using TMPro;

namespace Om
{
    public class PlayerContextElem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public string PlayerName => _text.text = name;

        public void Assign(string name) => _text.text = name;
    }
}