/*
 * RandomNameGenerator.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 29/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using System.Collections.Generic;
using UnityEngine;

namespace Brop
{
    public static class RandomNameGenerator
    {
        private static readonly List<string> firstNames = new List<string>
        {
            "John", "Jane", "Alex", "Emily", "Chris", "Katie", "Michael", "Sarah",
            "William", "Elizabeth", "Henry", "Victoria", "George", "Charlotte", "Edward", "Isabella"
        };

        private static readonly List<string> adjectives = new List<string>
        {
            "Brave", "Bold", "Strong", "Wise", "Just", "Kind", "Great", "Noble",
            "Mighty", "Gentle", "Fierce", "Radiant", "Merciful", "Valiant", "Fearless", "Proud"
        };

        public static string Generate()
        {
            string randomFirstName = firstNames[Random.Range(0, firstNames.Count)];
            string randomAdjective = adjectives[Random.Range(0, adjectives.Count)];
            return $"{randomFirstName}";
            //return $"{randomFirstName} The {randomAdjective}";
        }
    }
}