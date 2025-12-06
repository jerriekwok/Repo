using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        //进度条到第二阶段的一半时显示
        bool show = stoveCounter.isFried() && e.progressNormalized >= burnShowProgressAmount;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

