using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RPGcontroller : MonoBehaviour
{
    public void OnJump(InputValue value)
    {
        bool isActionPressd = value.isPressed;
        Debug.Log(isActionPressd);
    }

    void OnMove(InputValue value)
    {
        Vector2 moveValue = GetVector2Normalize(value.Get<Vector2>());
        Debug.Log(moveValue);
    }

    public Vector2 GetVector2Normalize(Vector2 vector2)
    {
        return vector2.normalized;
    }
}
