using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;//脚步的节奏控制器
    private float footstepTimerMax = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayerFootstepsSound(player.transform.position, volume);
            }
           
        }
    }
}

