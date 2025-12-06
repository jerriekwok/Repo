using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button playAgainButton;

    private void Awake()
    {

        //按钮监听
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        playAgainButton.onClick.AddListener(() =>
        {

            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.GameScene);

            
        });

        
    }

    private void Start()
    {
        //注册游戏状态变化时间
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        

        //初始化隐藏
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();

            //保存分数
            int score = DeliveryManager.Instance.GetSuccessfulRecipesAmount();
            LeaderboardManager.AddScore(score);

            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        mainMenuButton.Select();

       
    }
}

