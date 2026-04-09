using UnityEngine;

public class Grasshopper : SubzoneEnemy
{
    public enum GrasshopperState { Idle, Jumping }

    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float jumpForceX = 4f;
    [SerializeField] private float jumpForceY = 8f;
    [SerializeField] private float restDuration = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.15f;

    private GrasshopperState _state = GrasshopperState.Idle;
    private float _restTimer;
    private bool _hasLeftGround;
    private CapsuleCollider2D _collider;

    public GrasshopperState State => _state;

    public override void Awake()
    {
        base.Awake();
        _collider = GetComponent<CapsuleCollider2D>();
        _restTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (_isDying)
        {
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            rigidBody.velocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (_state)
        {
            case GrasshopperState.Idle:
                _restTimer -= Time.deltaTime;

                if (distanceToPlayer <= detectionRange && _restTimer <= 0f)
                {
                    FacePlayer();
                    Jump();
                }
                break;

            case GrasshopperState.Jumping:
                if (!_hasLeftGround && !IsGrounded())
                {
                    _hasLeftGround = true;
                }

                if (_hasLeftGround && IsGrounded())
                {
                    Land();
                }
                break;
        }
    }

    private void Jump()
    {
        _state = GrasshopperState.Jumping;
        _hasLeftGround = false;
        _animator.SetBool("IsJumping", true);

        float directionX = Mathf.Sign(player.position.x - transform.position.x);
        rigidBody.AddForce(new Vector2(directionX * jumpForceX, jumpForceY), ForceMode2D.Impulse);
    }

    private void Land()
    {
        _state = GrasshopperState.Idle;
        _hasLeftGround = false;
        _restTimer = restDuration;
        _animator.SetBool("IsJumping", false);

        rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
    }

    private void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;
        if ((direction > 0f && transform.localScale.x > 0f) ||
            (direction < 0f && transform.localScale.x < 0f))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + _collider.offset - new Vector2(0f, _collider.size.y * 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }
}
