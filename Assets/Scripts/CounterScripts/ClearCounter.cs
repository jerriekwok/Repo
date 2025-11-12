using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
  
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kichenObject here
            if (player.HasKitchenObject())
            {
                //player手上有kitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                    //plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }          
                }
                else
                {
                    //player手中没有物品,判断桌台的物品是否为plate
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {
                        //counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient( player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                            
                        }
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
   
}
        

        
