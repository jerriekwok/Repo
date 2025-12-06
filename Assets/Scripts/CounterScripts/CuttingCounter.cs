using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public static event EventHandler OnAnyCut;//静态事件 用于播放音效
    public event EventHandler OnCut;//切菜事件 用于动画播放

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;//切菜计数器


    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kichenObject here
            if (player.HasKitchenObject())
            {
                //判断玩家手中的kitchenobject能不能切
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //player手上有kitchenObject
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.CuttingProgressMax
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
                    }
                }
            }
            else
            {
                //player手中无kitchenObject
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.CuttingProgressMax
            });

            if (cuttingProgress >=  cuttingRecipeSO.CuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();//销毁
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);//生成
            }
           
          
        }
        
    }

    //判断是否存在可放上cuttingcounter进行F动作的物品
    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    //从对应的input的kitchenObjectSO 得到output的kichenObjectSO
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.Output;
        }
        return null;
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.Input == kitchenObjectSO)
            {
                return cuttingRecipeSO;
            }

        }
        return null;
    }
}

