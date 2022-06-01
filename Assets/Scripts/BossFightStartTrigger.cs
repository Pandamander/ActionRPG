using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStartTrigger : MonoBehaviour
{
    public BossFightManager manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            manager.BeginBossFight();
        }
    }

}
