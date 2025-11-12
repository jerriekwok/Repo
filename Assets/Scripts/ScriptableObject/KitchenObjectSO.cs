using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject 
{
    [field: SerializeField] public Transform prefab { get; private set; }
    [field: SerializeField] public string objectName { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
}
