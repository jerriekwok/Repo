using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {           
                Debug.Log("Interaction!");

            KitchenObject.SpawnKitchenObject(kitchenObjectSO,player);
                //触发事件
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        
        
    }
  
}

