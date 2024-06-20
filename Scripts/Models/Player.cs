/*
 * Player.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 14/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using Mirror;

namespace Brop.Models
{
    public class Player
    {
        public string Name { get; private set; }
        public NetworkConnection Connection { get; private set; }

        public Player(string name, NetworkConnection connection)
        {
            Name = name;
            Connection = connection;
        }
    }
}