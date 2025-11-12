using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {

            //Debug.Log("当前拿着的物体: " + player.GetKitchenObject().GetKitchenObjectSO().name);

            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //Debug.Log("plateKitchenObject 是否为 null？" + (plateKitchenObject == null));


                //和DliveryManager进行菜单匹配
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
               
            }
        }
    }
}
