/*
 * LoginUIController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 06/08/2024
 * Contact: c.dansembourg@icloud.com
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brop
{
    public class LoginUIController : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _usernameInputField;

        [SerializeField]
        private TMP_InputField _passwordInputField;

        [SerializeField]
        private Toggle _passwordVisibilityToggle;

        [SerializeField]
        private Toggle _rememberToggle;

        [SerializeField]
        private Button _loginButton;

        [SerializeField]
        private Button _registerButton;

        private List<Selectable> _navigationOrder;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                NextHighlight();
        }

        private void NextHighlight()
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            Selectable currentSelectable = currentSelected?.GetComponent<Selectable>();

            if (currentSelectable != null)
            {
                int currentIndex = _navigationOrder.IndexOf(currentSelectable);
                int nextIndex = (currentIndex + 1) % _navigationOrder.Count;
                _navigationOrder[nextIndex].Select();
            }
            else
                _navigationOrder[0].Select();
        }

        private void InitializeUI()
        {
            OnPasswordVisibilty();
            InitializeNavigationOrder();
        }

        public void OnPasswordVisibilty()
        {
            if (_passwordVisibilityToggle.isOn)
                _passwordInputField.contentType = TMP_InputField.ContentType.Password;
            else
                _passwordInputField.contentType = TMP_InputField.ContentType.Standard;

            RefreshInputField(_passwordInputField);
        }

        private void RefreshInputField(TMP_InputField inputField)
        {
            string currentText = inputField.text;

            inputField.text = string.Empty;
            inputField.text = currentText;

            inputField.caretPosition = currentText.Length;
        }

        private void InitializeNavigationOrder()
        {
            _navigationOrder = new List<Selectable>
            {
                _usernameInputField,
                _passwordInputField,
                _loginButton,
                _registerButton
            };
        }
    }
}