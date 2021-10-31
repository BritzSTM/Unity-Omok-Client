using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Om
{
    [RequireComponent(typeof(Image))]
    public class Emoticon : MonoBehaviour, IPointerClickHandler
    {
        public event UnityAction<Emoticon> OnClick;

        [SerializeField] private string _displayName = "";
        public string DisplayName => string.IsNullOrEmpty(_displayName) ? _image.sprite.name : _displayName;

        private Image _image;
        protected virtual void Awake()
        {
            _image = GetComponent<Image>();
        }

        protected void SetSprite(Sprite sprite) => _image.sprite = sprite;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}