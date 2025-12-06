using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    [field:SerializeField]public KitchenObjectSO Input { get; private set; }
    [field:SerializeField]public KitchenObjectSO Output { get; private set; }
    public int CuttingProgressMax;
}

