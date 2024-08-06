/*
 * MatchRecording.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 01/08/2024
 * Contact: c.dansembourg@icloud.com
 */

using System.Collections.Generic;

namespace Brop
{
    public class MatchRecording
    {
        public string Winner;
        public string Map;
        public List<BallRecording> BallRecordings = new List<BallRecording>();
    }
}