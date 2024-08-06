/*
 * LocalSettingsController.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 31/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using UnityEngine;

namespace Brop
{
    public class LocalSettingsController : MonoBehaviour
    {
        private void Awake()
        {
            Screen.SetResolution(1024, 640, false);
            Application.targetFrameRate = 45;
        }
    }
}