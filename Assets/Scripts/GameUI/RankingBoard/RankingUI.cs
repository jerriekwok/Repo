using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    [SerializeField] Transform highscoreEntryContianer;
    [SerializeField] Transform highscoreEntryTemplate;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI posText;

    [SerializeField] Button backButton;
    

    private int MaxEntries = 5;
    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            Hide();
        });

        //初始化面板与模板Template隐藏
        Hide();
        highscoreEntryTemplate.gameObject.SetActive(false);

    }

    private void Start()
    {
        
        ShowLeaderboard();
    }

    private void ShowLeaderboard()
    {
        foreach (Transform child in highscoreEntryContianer)
        {
            if (child != highscoreEntryTemplate)
            {
                Destroy(child.gameObject);//先清空旧的条目
            }
        }

        //读取排行榜数据
        LeaderboardData data = LeaderboardManager.LoadLeaderboard();

        int displayCount = Mathf.Min(data.scores.Count, MaxEntries);
        //生成排行榜数据
        for (int i = 0; i < displayCount; i++)
        {
            ScoreEntry entry = data.scores[i];
            Transform entryTransform = Instantiate(highscoreEntryTemplate, highscoreEntryContianer);
            entryTransform.gameObject.SetActive(true);
             
            TextMeshProUGUI posText = entryTransform.Find("posText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>();

            posText.text = GetRankString(i + 1) ;
            scoreText.text = entry.score.ToString();
        }

        Debug.Log($"当前排行榜共 {data.scores.Count} 条");
        Debug.Log($"第一名分数：{data.scores[0].score}");

    }
    private string GetRankString(int rank)
    {
        switch (rank)
        {
            case 1: return "1st";
            case 2: return "2nd";
            case 3: return "3rd";
            default: return rank + "th";
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        backButton.Select();
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
   
}
