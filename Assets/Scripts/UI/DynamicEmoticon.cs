using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Om
{
    [RequireComponent(typeof(Image))]
    public class DynamicEmoticon : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _interval = 0.15f;

        private Image _image;
        private float _accDeltaTime;
        private int _spriteIndex;

        private void Awake()
        {
            Debug.Assert(_sprites != null && _sprites.Length > 1);

            _image = GetComponent<Image>();
        }

        private void Update()
        {
            _accDeltaTime += Time.deltaTime;

            if (_accDeltaTime < _interval)
                return;

            _accDeltaTime = 0.0f;
            _spriteIndex = ++_spriteIndex % _sprites.Length;
            _image.sprite = _sprites[_spriteIndex];
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