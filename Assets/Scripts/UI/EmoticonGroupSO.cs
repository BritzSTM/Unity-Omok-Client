using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Om
{
    [CreateAssetMenu(fileName = "new emoticon group so", menuName = "Om/EmoticonGroup")]
    public class EmoticonGroupSO : ScriptableObject
    {
        public string DisplayName;
        public Sprite CoverSprite;
        public Emoticon[] Emoticons;
    }
}