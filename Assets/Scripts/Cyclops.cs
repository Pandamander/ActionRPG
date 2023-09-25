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
        //Debug.Log("Boulder Spawn: " + boulderSpawn.transform.position);
    }

    public void Move(float xSpeed)
    {
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
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
        Debug.Log("ThrowBoulder");
        _animator.SetTrigger("throwBoulder");
        Instantiate(boulder, boulderSpawn.position, Quaternion.identity);
    }

    private void Smash()
    {
    }

    private void Swipe()
    {
    }

    public void Attack(AttackType type)
    {
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
