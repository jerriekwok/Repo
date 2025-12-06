using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;//静态事件 用于音效 

    [SerializeField] private Transform counterTopPoint;//物品生成的点位

    private KitchenObject kitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    public virtual void Interact(Player player)//E
    {
        Debug.Log("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player)//F
    {
        Debug.Log("BaseCounter.InteractAlternate");
    }
    public Transform GetKitchenObjectFollowTransfrom()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
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

