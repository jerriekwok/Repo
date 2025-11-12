using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    #region 状态切换时的动画事件
    public event EventHandler<OnStateChangedArgs> OnStateChanged;
    public class OnStateChangedArgs : EventArgs
    {
        public State state;
    }

    #endregion
    public enum State//关于肉的状态机
    {
        Idle,//空
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;//包含肉排的input的和output 生肉到熟肉
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;//熟肉到糊肉

    

    private float fryingTimer;//生肉到熟肉的计时器
    private float burningTimer;//熟肉到糊肉的计时器
    private FryingRecipeSO fryingRecipeSO; 
    private BurningRecipeSO burningRecipeSO;
    private State state;
    private void Start()
    {
        state = State.Idle;//初始化状态
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    //Debug.Log("frying状态");
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.FryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.FryingTimerMax)
                    {                        
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.Output, this);
                        //Debug.Log("object fried!");
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedArgs {
                             state = this.state
                        });

                    }
                    break;
                case State.Fried:
                    
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.BurningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.BurningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.Output, this);
                        //Debug.Log("object burn!");
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedArgs
                        {
                            state = this.state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                    
                    break;
                case State.Burned:   
                    break;
            }
            //Debug.Log(state);
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kichenObject here
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player手上有kitchenObject
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;//一旦开始交互就处于Frying状态
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedArgs
                    {
                        state = this.state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.FryingTimerMax
                    });
                }

            }
            else
            {
                //player手中无kitchenObject
            }
        }
        else
        {
            //There is a kichenObject here
            if (player.HasKitchenObject())
            {
                //player手中有kitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //检测player手中物品是否为plate
                    plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedArgs
                        {
                            state = this.state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }

            }
            else
            {
                //player手中无kitchenObject
                this.GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;//灶台切换状态

                OnStateChanged?.Invoke(this, new OnStateChangedArgs
                {
                    state = this.state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeWithInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeWithInput(kitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.Output;
        }
        return null;
    }
    private FryingRecipeSO GetFryingRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.Input == kitchenObjectSO)
            {
                return fryingRecipeSO;
            }

        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.Input == kitchenObjectSO)
            {
                return burningRecipeSO;
            }

        }
        return null;
    }

    public bool isFried()
    {
        return state == State.Fried;
    }
}
