using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 送餐管理器
/// </summary>
public class DeliveryManager : MonoBehaviour
{
    //声明事件
    public event EventHandler OnRecipeSpawned;//菜品生成
    public event EventHandler OnRecipeComplate;//传菜完成
    public event EventHandler OnRecipeSuccess;//传菜成功
    public event EventHandler OnRecipeFailed;//传菜失败

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeSOList recipeListSO;//菜单列表

    private List<RecipeSO> waitingRecipeSOList;//等待中的菜单列表
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int SuccessfulRecipesAmount;//成功完成的订单数量

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        #region 生成等待菜单的逻辑
        if (KitchenGameManager.Instance.IsGamePlaying())
        {
            spawnRecipeTimer -= Time.deltaTime;
            if (spawnRecipeTimer <= 0f)
            {
                spawnRecipeTimer = spawnRecipeTimerMax;

                if (waitingRecipeSOList.Count < waitingRecipesMax)
                {
                    //随机获取菜品
                    RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                    Debug.Log(waitingRecipeSO.recipeName);
                    //添加至等待列表
                    waitingRecipeSOList.Add(waitingRecipeSO);

                    //触发事件
                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
      
        #endregion

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count ; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {//先对比配方数目
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {//循环菜谱中的菜品
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {//循环餐盘中的菜品
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true; 

                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    Debug.Log("player delivered the correct recipe");

                    SuccessfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);//移除等待菜品

                    //触发事件
                    OnRecipeComplate?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        Debug.Log("player did not delivered the correct recipe");//没有匹配的菜品，出餐失败
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetwaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return SuccessfulRecipesAmount;
    }
}
