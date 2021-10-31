using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Om
{
    [RequireComponent(typeof(Image))]
    public class DynamicEmoticon : Emoticon
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _interval = 0.15f;

        private float _accDeltaTime;
        private int _spriteIndex;

        protected override void Awake()
        {
            base.Awake();

            Debug.Assert(_sprites != null && _sprites.Length > 1);
        }

        private void Update()
        {
            _accDeltaTime += Time.deltaTime;

            if (_accDeltaTime < _interval)
                return;

            _accDeltaTime = 0.0f;
            _spriteIndex = ++_spriteIndex % _sprites.Length;

            SetSprite(_sprites[_spriteIndex]);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying || _sprites.Length < 1)
                return;

            EditorApplication.delayCall += () =>
            {
                if (_sprites == null)
                    return;

                var obj = this;
                if (obj == null)
                    return;

                var imgComponent = GetComponent<Image>();
                if (imgComponent == null)
                    return;

                var serializedObj = new SerializedObject(imgComponent);
                serializedObj.Update();

                var spriteProperty = serializedObj.FindProperty("m_Sprite");
                spriteProperty.objectReferenceValue = _sprites[0];

                serializedObj.ApplyModifiedProperties();
            };
        }
#endif
    }
}