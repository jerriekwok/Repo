using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        settingButton.onClick.AddListener(() =>
        {
            Hide();
            SettingsUI.Instance.Show(Show);
        });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenManager_OnGameUnpaused;

        Hide();
    }

    private void KitchenManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select();//处于被选中状态
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}

