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
    [SerializeField] private Transform swipeAttackPoint;
    [SerializeField] private DamageFlash damageFlash;

    private Animator _animator;
    private Rigidbody2D _rb;
    private bool shouldWalk = true;
    public int health { get; private set; } = 14;
    private SpriteRenderer _spriteRenderer;
    private bool isAttacking = false;
    private bool isKneeling = false;
    private Coroutine kneelFlash;
    private Collider2D _collider;
    private const int PLAYER_COLLISION_LAYER = 1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
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
            _animator.SetBool("isWalking", true);
            _rb.velocity = new Vector2(xSpeed, _rb.velocity.y);
        }
    }

    public void FlipSprite()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    public void Roar()
    {
        _animator.SetTrigger("roar");
    }

    private void ThrowBoulder()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("throwBoulder");
    }

    public void ThrowBoulderAnimationSpawnFrame()
    {
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
        isAttacking = false;
    }

    private void Smash()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("smash");
    }

    private IEnumerator ResumeWalkAfterSmash()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(ResumeWalking());
    }

    public void SmashAnimationHitGroundFrame()
    {
        cameraShake.ShakeCamera(0.25f, 5f);
        audioManager.PlayDamage();
        SpawnSand();
        StartCoroutine(ResumeWalkAfterSmash());
    }

    private void SpawnSand()
    {
        sandWave.SpawnWave();
    }

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
        yield return new WaitForSeconds(0.3f);
        SwipeMeleeAttack();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ResumeWalking());
    }

    public void SwipeMeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            swipeAttackPoint.position,
            0.5f,
            LayerMask.GetMask("TransparentFX")
        );

        foreach (Collider2D c in hitEnemies)
        {
            if (c.gameObject.TryGetComponent<IDamageable>(out var enemy))
            {
                enemy.Damage(2, Utilities.DamageDirection(gameObject, c.gameObject));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(swipeAttackPoint.position, 0.5f);
    }

    public void Attack(AttackType type)
    {
        if (isAttacking) { return; }
        isAttacking = true;
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

    public void Die()
    {
        shouldWalk = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("death");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isKneeling)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var player))
            {
                player.Damage(1, Utilities.DamageDirection(gameObject, collision.gameObject));
            }
        }
    }

    // IDamageable
    public void Damage(int damage, float damageDirection)
    {
        audioManager.PlayAttackHit();
        subzoneHUD.ReduceBossHealthMeter(damage);
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }

        if (health > 0)
        {
            damageFlash.Flash();
        }
    }
}
