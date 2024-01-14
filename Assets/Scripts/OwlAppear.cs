using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlAppear : MonoBehaviour
{
    [SerializeField] private SubzoneOwl owl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            owl.AppearTrigger();
        }
    }
}
