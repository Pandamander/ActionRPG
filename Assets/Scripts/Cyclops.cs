using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : MonoBehaviour
{
    public enum AttackType
    {
        ThrowBoulder, Smash, Swipe
    }

    public AttackType[] attackTypes = { AttackType.ThrowBoulder };

    [SerializeField] private GameObject boulder;
    [SerializeField] private Transform boulderSpawn;
    private Animator _animator;
    private Rigidbody2D rb;
    private bool shouldWalk = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
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
}
