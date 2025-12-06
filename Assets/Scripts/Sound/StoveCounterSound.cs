using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        audioSource.volume = SoundManager.Instance.volume;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.isFried() && e.progressNormalized >= burnShowProgressAmount;

    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedArgs e)
    {
        bool playSound = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried);
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                //播放警报音
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }

        
        if (SettingsUI.Instance.isPanelActive())
        {
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.volume = SoundManager.Instance.volume;
        }
    }

}

