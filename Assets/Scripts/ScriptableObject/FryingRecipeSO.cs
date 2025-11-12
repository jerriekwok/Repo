using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    [field:SerializeField]public KitchenObjectSO Input { get; private set; }
    [field:SerializeField]public KitchenObjectSO Output { get; private set; }
    public float FryingTimerMax;
}
