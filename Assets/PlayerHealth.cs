using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int maxHealth = 10;

    private void Start()
    {
        health = maxHealth;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DamagePlayer") // Run into a building
        {
            DealDamage(1);
            Debug.Log("Damage! HP:" + health);
        }
    }

    public void DealDamage(int amt)
    {
        health -= amt;
        GetComponent<SpriteRenderer>().color = new Color(1f, health/maxHealth, health / maxHealth, 1f);

        if (health <= 0)
        {
            FindObjectOfType<GetCaught>().EndOfGame();
            // Blow up car
        }
    }

}


/*
Collide with building - DONE
Health that goes down - DONE
Show health
Make blinking
Make smoking
Change blinking frequency
Car blows up when health is zero

 */