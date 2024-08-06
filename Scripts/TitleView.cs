/*
 * TitleView.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 13/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using TMPro;
using UnityEngine;

namespace Brop.Views
{
    public class TitleView : MonoBehaviour
    {
        private TMP_Text _textComponent;
        private float _timer;

        [SerializeField]
        private float _speed = 0.2f;

        private void Start()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _timer += Time.deltaTime * _speed;
            _textComponent.color = GetFullColor(_timer);
        }

        private Color GetFullColor(float time)
        {
            float r = Mathf.Abs(Mathf.Sin(time));
            float g = Mathf.Abs(Mathf.Sin(time + Mathf.PI / 3));
            float b = Mathf.Abs(Mathf.Sin(time + 2 * Mathf.PI / 3));

            return new Color(r, g, b);
        }
    }
}