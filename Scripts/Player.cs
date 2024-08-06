/*
 * Player.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;
using System;

namespace Brop.Models
{
    [Serializable]
    public class Player
    {
        public string Name;
        public NetworkIdentity NetworkIdentity;
        public PlayerProfile PlayerProfile;
    }
}