using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//数据管理辅助类
public static class LeaderboardManager
{
    private const string LEADBOARD_KEY = "LeaderboardData";

    //存储数据
    public static void SaveLeaderboard(LeaderboardData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(LEADBOARD_KEY, json);
        PlayerPrefs.Save();
    }

    //加载数据
    public static LeaderboardData LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LEADBOARD_KEY))
        {
            string json = PlayerPrefs.GetString(LEADBOARD_KEY,"");
            //解析json
            return JsonUtility.FromJson<LeaderboardData>(json);

        }
        return new LeaderboardData();
    }

    public static void AddScore(int score)
    {
        LeaderboardData data = LoadLeaderboard();
        data.scores.Add(new ScoreEntry 
        { 
            score = score 
        });

        //从高到低排序
        data.scores.Sort((a, b) => b.score.CompareTo(a.score));

        //限制最大数量
        if (data.scores.Count > 10)
        {
            data.scores.RemoveRange(10, data.scores.Count - 10);
        }

        SaveLeaderboard(data);

        Debug.Log($"已保存分数:{score}");
    }
}
