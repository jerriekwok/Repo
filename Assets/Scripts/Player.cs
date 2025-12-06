using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;//拾取东西的事件 用于触发音效

    #region 指向柜台闪烁的事件
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs//用于封装事件触发时传递数据
    {
        public BaseCounter SelectedCounter;
    }
    #endregion

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;//交互事件脚本
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;//玩家身上物品放置坐标

    private BaseCounter selectedCounter;
    private float rotateSpeed = 10f;
    private bool isWalking;
    private float playerHeight = 2f;
    private float playerRadius = .7f;
    private Vector3 lastInteractDir;
    private KitchenObject kitchenObject;
    private void Awake()
    {
        if (Instance = null)
        {
            Debug.LogError("instance 引用为空");
        }
        Instance = this;
    }
    private void Start()
    {
        //订阅事件
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())//是否处在游戏状态
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())//是否处在游戏状态
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions()//柜子射线交互
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;
        if(moveDir != Vector3.zero)
        {
            //获取停止移动之前最近的一次moveDir
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,counterLayerMask))
        {
            //与柜子的交互
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //得到空柜子
                if (baseCounter != selectedCounter)
                {
                    //得到选中的柜子
                    selectedCounter = baseCounter;

                    SetSelectedCounter(selectedCounter);

                }
            }
            else
            {
                SetSelectedCounter(null);//得到了物体但不是ClearCounter
            }

        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();//获取到玩家输入的单位向量 转化成vector3
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);//XZ平面移动，Y是高度
        float moveDistance = Time.deltaTime * moveSpeed;//计算移动距离
        //碰撞检测
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHeight, playerRadius, moveDir, moveDistance);
        #region 解决对角移动的问题
        if (!canMove)
        {
            //尝试在沿x移动
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f ) && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //可以沿x方向移动
                moveDir = moveDirX;
            }
            else
            {
                //不能沿x方向移动,尝试在Z方向移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + transform.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //可以沿z方向移动
                    moveDir = moveDirZ;
                }
                else
                {
                    //不能向任何方向移动，考虑周围都有碰撞体
                }
            }
        }
        #endregion
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }//玩家移动的逻辑
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = this.selectedCounter
        });
    }//设置选中的柜台

    public Transform GetKitchenObjectFollowTransfrom()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()//清除物品
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()//检测是否存在物品
    {
        return kitchenObject != null;
    }
}

