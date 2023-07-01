using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("melee OnTriggerEnter2D");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("melee OnCollisionEnter2D");
    }
}
