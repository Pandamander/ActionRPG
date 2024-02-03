using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class SubzoneOwl : SubzoneEnemy
{
    [SerializeField] private Transform player;
    [SerializeField] private BoxCollider2D _boxCollider;
    private Vector2 _target;
    private Vector2 _direction;
    private bool _flyTowardPlayer;
    private bool _hasBegunSwoopUp;
    private float _swoopUpTimer = 0f;
    private float _flyUpTimeToLive = 5f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if (_hasBegunSwoopUp)
        {
            _swoopUpTimer += Time.deltaTime;
            if (_swoopUpTimer >= _flyUpTimeToLive)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isDying) {  return; }

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

    public void AppearTrigger()
    {
        _boxCollider.enabled = false;
        _animator.SetBool("IsAppearing", true);
    }

    public void AppearAnimationComplete()
    {
        _animator.SetBool("IsFlying", true);
        _target = new Vector2(player.position.x, player.position.y - 2f);
        _direction = (new Vector2(_target.x, _target.y) - rigidBody.position).normalized;
        if (_direction.x > 0f) { FlipFacingDirection(); }
        _flyTowardPlayer = true;
    }

    private void FlipFacingDirection()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
