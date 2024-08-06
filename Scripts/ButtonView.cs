/*
 * ButtonView.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 13/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brop.Views
{
    public class ButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Color _textHoverColor;

        [SerializeField]
        private Sprite _hoverSprite;

        private TMP_Text _buttonText;
        private Image _buttonImage;
        private Color _originalColor;
        private Sprite _originalSprite;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _buttonText = GetComponentInChildren<TMP_Text>();
            _buttonImage = GetComponent<Image>();

            _originalColor = _buttonText.color;
            _originalSprite = _buttonImage.sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonText.color = _textHoverColor;
            _buttonImage.sprite = _hoverSprite;

            _audioSource.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonText.color = _originalColor;
            _buttonImage.sprite = _originalSprite;
        }
    }
}