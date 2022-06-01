using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { Attack, Defense, Health };
public class Powerup : MonoBehaviour
{
    public SubzoneAudioManager audioManager;
    public PowerupType powerupType;
    public string uniqueId;

    private void Update()
    {
        if (powerupType == PowerupType.Attack)
        {
            transform.Rotate(0f, 0f, 300f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            audioManager.PlayPowerup();
            switch (powerupType)
            {
                case PowerupType.Attack:
                    PlayerStats.UpgradetAttack();
                    break;
                case PowerupType.Defense:
                    PlayerStats.UpgradeDefense();
                    break;
                case PowerupType.Health:
                    PlayerStats.UpgradeHealth();
                    break;
            }
        }
    }
}