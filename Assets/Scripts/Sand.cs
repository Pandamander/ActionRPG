using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(0.417f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(1);
            }
        }
    }
}
