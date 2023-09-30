using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(1f);
            }
        }
    }
}
