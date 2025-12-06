using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;//声明事件
    public event EventHandler OnInteractAlternateAction;//声明事件 ： F键切菜
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;//事件 当按键重映射时

    private PlayerInputAction playerInputAction;
    
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        //手柄部分
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    private void Awake()
    {  
        Instance = this;

        //激活新输入系统
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        //订阅事件
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

        LoadBindings();

    }

    private void OnDestroy()
    {
        //Debug.Log("解绑事件释放资源");
        //解除事件
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        //关闭输入并释放资源

        playerInputAction.Player.Disable();
        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //触发暂停事件
        OnPauseAction?.Invoke(this, EventArgs.Empty);
        
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //触发事件
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //触发事件
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
   {
        Vector2 moveDir = playerInputAction.Player.Move.ReadValue<Vector2>();
        #region 旧输入系统的写法
        //if (Input.GetKey(KeyCode.A))
        //{
        //    moveDir.x = -1;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    moveDir.x = 1;
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    moveDir.z = 1;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    moveDir.z = -1;
        //}
        #endregion

        moveDir = moveDir.normalized;
        return moveDir;
    }

    //用于映射按键文本
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();
        }
        return null;
    }

    //按键重映射
    public void RebindBinding(Binding binding,Action onActionRebound)
    {
        playerInputAction.Player.Disable();//禁用输入防止误触

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
                
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;

        }

        inputAction.PerformInteractiveRebinding(bindingIndex)//创建ReBinding对象
            .OnComplete(callback =>
            {
                //Debug.Log(callback.action.bindings[1].path);
                //Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose();
                playerInputAction.Player.Enable();//重启输入
                onActionRebound();

                SaveBindings();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();//开始监听 等待玩家输入
    }
    

    public void SaveBindings()
    {
        //序列化玩家重映射的的部分，生成JSON字符串
        string rebinds = playerInputAction.SaveBindingOverridesAsJson();
        //持久化存储
        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, rebinds);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))//判断是否存在保存项
        {
            string rebinds = PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS);
            playerInputAction.LoadBindingOverridesFromJson(rebinds);//解析JSON并应用回系统
        }
    }

    public void ResetBindings()
    {
        PlayerPrefs.DeleteKey(PLAYER_PREFS_BINDINGS);
        playerInputAction.RemoveAllBindingOverrides();
    }

}

