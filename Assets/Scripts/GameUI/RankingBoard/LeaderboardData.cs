using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]//可序列化
public class ScoreEntry
{
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}

