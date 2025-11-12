using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }
    [SerializeField] private mode Mode;
    private void LateUpdate()
    {
        switch (Mode)
        {
            case mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case mode.LookAtInverted:
                Vector3 dirFormCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFormCamera);
                break;
            case mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
