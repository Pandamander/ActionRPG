using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : MonoBehaviour, IDamageable
{
    public enum AttackType
    {
        ThrowBoulder, Smash, Swipe
    }

    public AttackType[] attackTypes = { AttackType.ThrowBoulder, AttackType.Smash, AttackType.Swipe };

    [SerializeField] private GameObject boulder;
    [SerializeField] private Transform boulderSpawn;
    [SerializeField] private SubzoneHUD subzoneHUD;
    [SerializeField] private SubzoneAudioManager audioManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private Transform sandSpawn1;
    [SerializeField] private Transform sandSpawn2;
    [SerializeField] private GameObject sand;

    private Animator _animator;
    private Rigidbody2D _rb;
    private bool shouldWalk = true;
    private int _health = 14;
    private SpriteRenderer _spriteRenderer;
    private bool _shouldSpawnSand = false;

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
        Vector2 jump = Vector2.zero;
        if (playerTransform.position.x > transform.position.x)
        {
            jump = Vector2.right;
        } else
        {
            jump = Vector2.left;
        }

        _rb.AddForce(5f * new Vector2(jump.x, 1.92f), ForceMode2D.Impulse);
        _shouldSpawnSand = true;
        StartCoroutine(SmashAttack());
    }

    private IEnumerator SmashAttack()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(ResumeWalking());
    }

    private void SpawnSand()
    {
        Rigidbody2D sand1rb = Instantiate(sand, sandSpawn1.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Rigidbody2D sand2rb = Instantiate(sand, sandSpawn2.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 sandDir = Vector2.zero;
        if (playerTransform.position.x > transform.position.x)
        {
            sandDir = Vector2.right;
        }
        else
        {
            sandDir = Vector2.left;
        }
        sand1rb.AddForce(new Vector2(sandDir.x * 4f, 7f), ForceMode2D.Impulse);
        sand2rb.AddForce(new Vector2(sandDir.x * -1 * 4f, 7f), ForceMode2D.Impulse);
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
        if (collision.gameObject.CompareTag("Ground") && _shouldSpawnSand)
        {
            _rb.velocity = Vector2.zero;
            _shouldSpawnSand = false;
            cameraShake.ShakeCamera(0.25f, 2f);
            audioManager.PlayDamage();
            SpawnSand();
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
        _health -= (int)damage;
        if (_health <= 0f)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        StartCoroutine(TakeDamage());
    }
}
