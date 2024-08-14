using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger2 : MonoBehaviour
{
    public PlayerManager player;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            player.GadiunFight();
        }
    }
}
