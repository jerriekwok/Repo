using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button rankingButton;

    [SerializeField] private RankingUI rankingUI;

    private void Awake()
    {

        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        rankingButton.onClick.AddListener(() =>
        {
            rankingUI.Show();
        });

        Time.timeScale = 1f;  
    }
}
