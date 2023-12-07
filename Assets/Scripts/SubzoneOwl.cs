using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class SubzoneOwl : SubzoneEnemy
{
    [SerializeField] private Transform player;
    private BoxCollider2D _boxCollider;
    private Vector2 _target;
    private Vector2 _direction;
    private bool _flyTowardPlayer;
    private bool _hasBegunSwoopUp;
    private float _swoopUpTimer = 0f;
    private float _flyUpTimeToLive = 5f;

    public override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Update()
    {
        base.Update();

        if (_hasBegunSwoopUp)
        {
            _swoopUpTimer += Time.deltaTime;
            if (_swoopUpTimer >= _flyUpTimeToLive)
            {
                Debug.Log("Destroying owl");
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_flyTowardPlayer)
        {
            rigidBody.MovePosition(rigidBody.position + _direction * moveSpeed * Time.fixedDeltaTime);

            if (!_hasBegunSwoopUp && rigidBody.position.y <= _target.y)
            {
                _hasBegunSwoopUp = true;
                _direction = new Vector2(_direction.y, -_direction.x);
            }
        }         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _boxCollider.enabled = false;
            _animator.SetBool("IsAppearing", true);
        }
    }

    public void AppearAnimationComplete()
    {
        _animator.SetBool("IsFlying", true);
        _target = new Vector2(player.position.x, player.position.y - 1f);
        _direction = (new Vector2(_target.x, _target.y) - rigidBody.position).normalized;
        _flyTowardPlayer = true;
    }
}
