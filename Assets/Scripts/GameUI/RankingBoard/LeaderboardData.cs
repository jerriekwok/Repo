using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]//ø…–Ú¡–ªØ
public class ScoreEntry
{
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}
