using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlindText : MonoBehaviour
{
   
    public Text text; // 绑定你的TMP文本组件
    public float blinkSpeed = 1f; // 闪烁速度，越大越快
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
    }
    private IEnumerator Blink()
    {
        Color color = text.color;
        while (true)
        {

            // 利用PingPong让alpha在0~1之间往复变化
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            
            color.a = alpha;
            text.color = color;
   
            yield return null;
        }
    }

}
