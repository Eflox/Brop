/*
 * RegisterUIController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 06/08/2024
 * Contact: c.dansembourg@icloud.com
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brop
{
    public class RegisterUIController : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _usernameInputField;

        [SerializeField]
        private TMP_InputField _emailInputField;

        [SerializeField]
        private TMP_InputField _passwordInputField;

        [SerializeField]
        private TMP_InputField _passwordConfirmationInputField;

        [SerializeField]
        private Button _registerButton;
    }
}