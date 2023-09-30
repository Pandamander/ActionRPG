using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private int bounceCount = 0;
    private Animator boulderAnimator;
    public SubzoneAudioManager audioManager;
    public SubzoneHUD hud;
    public CameraShake cameraShake;

    private void Awake()
    {
        boulderAnimator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.y);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.333f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            audioManager.PlayDamage();
            cameraShake.ShakeCamera(0.25f, 2f);
            
            bounceCount++;
            if (bounceCount == 2)
            {
                boulderAnimator.SetTrigger("Break");
                StartCoroutine(Destroy());
            }
        } else if (collision.collider.CompareTag("Player")) {
            boulderAnimator.SetTrigger("Break");
            StartCoroutine(Destroy());

            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(2f);
            }
        }
    }
}
