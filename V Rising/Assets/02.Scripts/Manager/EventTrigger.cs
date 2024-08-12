using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{

    public Maja maja;
    public PlayerManager player;

    private bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            maja.target = player.transform;
            maja.talkSound = SoundManager.instance.ActiveOnShotSFXSound(Sound.AudioClipName.BossStart, maja.transform, Vector3.zero);
            active = true;
        }
    }
}
