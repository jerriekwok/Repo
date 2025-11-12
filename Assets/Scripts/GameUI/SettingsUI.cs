using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Button backButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;


    [SerializeField] private Transform presstoRebindKeyTransform;

    private Action onCloseButtonAction;

    private bool state = false;

    private void Awake()
    {
        Instance = this;

        backButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });
        moveUpButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Interact);
        });
        interactAlternateButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.InteractAlternate);
        });
        pauseButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Interact);
        });
        gamepadInteractAlternateButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.InteractAlternate);
        });
        gamepadPauseButton.onClick.AddListener(() => {
            Rebinding(GameInput.Binding.Pause);
        });
    }
    private void Start()
    {
        musicSlider.value = MusicManager.Instance.GetVolume();
        effectSlider.value = SoundManager.Instance.GetVolume();

        //注册监听
        musicSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.SetVolume(value);

        });
        effectSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.SetVolume(value);
        });
        

        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;//双重保险确保该面板的关闭
        UpdateVisual();
        HidePresstoRebindKeyTransform();
        Hide();
    }

    void Update()
    {
        //清空所有本地设置  用于调试
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs 已重置");
        }
    }

    public void UpdateVisual()
    {
        //text文本的按键映射
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        this.gameObject.SetActive(true);
        effectSlider.Select();

        state = true;
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);

        state = false;
    }

    public void ShowPresstoRebindKeyTransform()
    {
        presstoRebindKeyTransform.gameObject.SetActive(true);
        
    }
    public void HidePresstoRebindKeyTransform()
    {
        presstoRebindKeyTransform.gameObject.SetActive(false);
    }

    public void Rebinding(GameInput.Binding binding)
    {
        ShowPresstoRebindKeyTransform();
        GameInput.Instance.RebindBinding(binding,() => {
            HidePresstoRebindKeyTransform();
            UpdateVisual();
        });
        
    }

    public bool isPanelActive()
    {
        return state;
    }


}
