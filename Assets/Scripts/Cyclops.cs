using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : MonoBehaviour, IDamageable
{
    public enum AttackType
    {
        ThrowBoulder, Smash, Swipe
    }

    public AttackType[] autoAttackTypes = { AttackType.ThrowBoulder, AttackType.Smash };

    [SerializeField] private GameObject boulder;
    [SerializeField] private Transform boulderSpawn;
    [SerializeField] private SubzoneHUD subzoneHUD;
    [SerializeField] private SubzoneAudioManager audioManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private SandWave sandWave;

    private Animator _animator;
    private Rigidbody2D _rb;
    private bool shouldWalk = true;
    public int health { get; private set; } = 14;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerTransform.position.x > transform.position.x) && transform.localScale.x > 0)
        {
            FlipSprite();
        } else if ((playerTransform.position.x < transform.position.x) && transform.localScale.x < 0)
        {
            FlipSprite();
        }
    }

    public void Move(float xSpeed)
    {
        if (shouldWalk)
        {
            _rb.velocity = new Vector2(xSpeed, _rb.velocity.y);
        }
    }

    public void FlipSprite()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public void Walk()
    {
        _animator.SetBool("isWalking", true);
    }

    private void ThrowBoulder()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("throwBoulder");
        StartCoroutine(SpawnBoulder());
    }

    private IEnumerator SpawnBoulder()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject spawnedBoulder = Instantiate(boulder, boulderSpawn.position, Quaternion.identity);
        Boulder boulderInstance = spawnedBoulder.GetComponent<Boulder>();
        boulderInstance.audioManager = audioManager;
        boulderInstance.hud = subzoneHUD;
        boulderInstance.cameraShake = cameraShake;
        float direction = transform.localScale.x;
        spawnedBoulder.GetComponent<Rigidbody2D>().AddForce(5f * direction * Vector2.left, ForceMode2D.Impulse);
        StartCoroutine(ResumeWalking());
    }

    private IEnumerator ResumeWalking()
    {
        yield return new WaitForSeconds(0.5f);
        shouldWalk = true;
    }

    private void Smash()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("smash");
        StartCoroutine(SmashAttack());
    }

    private IEnumerator SmashAttack()
    {
        yield return new WaitForSeconds(1.2f);
        cameraShake.ShakeCamera(0.25f, 5f);
        audioManager.PlayDamage();
        SpawnSand();
        yield return new WaitForSeconds(1f);
        StartCoroutine(ResumeWalking());
    }

    private void SpawnSand()
    {
        sandWave.SpawnWave();
    }

    // TODO: ELLIOTT - Swipe does not damage player yet
    private void Swipe()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("swipe");
        StartCoroutine(SwipeAttack());
    }

    private IEnumerator SwipeAttack()
    {
        yield return new WaitForSeconds(0.2f);
        audioManager.PlayAttack();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ResumeWalking());
    }

    public void Attack(AttackType type)
    {
        Debug.Log("Attack: " + type);
        switch (type)
        {
            case AttackType.ThrowBoulder:
                ThrowBoulder(); break;
            case AttackType.Smash:
                Smash(); break;
            case AttackType.Swipe:
                Swipe(); break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(1f);
            }
        }
    }

    private IEnumerator TakeDamage()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        _spriteRenderer.color = Color.white;
    }

    // IDamageable
    public void Damage(float damage)
    {
        subzoneHUD.ReduceBossHealthMeter((int)damage);
        health -= (int)damage;
        if (health <= 0f)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        StartCoroutine(TakeDamage());
    }
}
