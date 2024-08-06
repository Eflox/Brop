/*
 * BallView.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using TMPro;
using UnityEngine;

namespace Brop.Views
{
    public class BallView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _playerNameText;

        public void SetName(string name)
        {
            _playerNameText.text = name;
        }
    }
}