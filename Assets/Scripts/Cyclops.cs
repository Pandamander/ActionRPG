using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : MonoBehaviour, IDamageable
{
    public enum AttackType
    {
        ThrowBoulder, Smash, Swipe
    }

    public AttackType[] attackTypes = { AttackType.ThrowBoulder };

    [SerializeField] private GameObject boulder;
    [SerializeField] private Transform boulderSpawn;
    [SerializeField] private SubzoneHUD subzoneHUD;
    [SerializeField] private SubzoneAudioManager audioManager;

    private Animator _animator;
    private Rigidbody2D _rb;
    private bool shouldWalk = true;
    private int _health = 14;
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
        Rigidbody2D boulderrb = Instantiate(boulder, boulderSpawn.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        float direction = transform.localScale.x;
        boulderrb.AddForce(5f * direction * Vector2.left, ForceMode2D.Impulse);
        StartCoroutine(ResumeWalking());
    }

    private IEnumerator ResumeWalking()
    {
        yield return new WaitForSeconds(0.5f);
        shouldWalk = true;
    }

    private void Smash()
    {
    }

    private void Swipe()
    {
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
        _health -= (int)damage;
        if (_health <= 0f)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        StartCoroutine(TakeDamage());
    }
}
