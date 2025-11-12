using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerController : MonoBehaviour
{
    public GameInputActions gameInputActions;
    public Transform targetFollowTransform;
    public Transform cameraTransform;
    public float moveSpeed = 2f;
    private Rigidbody playerRigidbody;

    Vector2 moveVector2 => gameInputActions.PC.Move.ReadValue<Vector2>();
    private void Awake()
    {
        //实例化脚本
        gameInputActions = new GameInputActions();

        
    }
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //开启脚本
        gameInputActions.PC.Enable();
        
    }
    private void OnDisable()
    {
        //关闭脚本
        gameInputActions.PC.Disable();
    }
    private void Update()
    {
        getCameraControlInput();
        getJumpInput();
        getMoveInput();
    }

    private void getMoveInput()
    {
        
        //判断是否按下对应的Move按键
        if (moveVector2 != Vector2.zero)
        {
            Debug.Log(moveVector2);

            transform.Translate(new Vector3(moveVector2.x, 0, moveVector2.y) * Time.deltaTime * moveSpeed, Space.World);
        }
    }

    private void getJumpInput()
    {
        bool isJump = gameInputActions.PC.Jump.IsPressed();
        if (isJump)
        {
            Debug.Log(isJump);

            playerRigidbody.AddForce(Vector2.up * 5);
        }
    }

    private void getCameraControlInput()
    {
        Vector2 cameraOffset = gameInputActions.PC.CameraControl.ReadValue<Vector2>();

        if (cameraOffset != Vector2.zero)
        {
            cameraTransform.RotateAround(targetFollowTransform.position, Vector3.up, cameraOffset.x);
            cameraTransform.RotateAround(targetFollowTransform.position, cameraTransform.right, cameraOffset.y);
        }
    }

}
