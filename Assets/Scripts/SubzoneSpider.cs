using UnityEngine;

public class SubzoneSpider : SubzoneEnemy
{
    public enum SpiderState { Idle, Attacking, Retreating }

    [SerializeField] private Transform player;
    [SerializeField] private Transform dropTarget;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float retreatRange = 7f;
    [SerializeField] private float dropDuration = 0.5f;
    [SerializeField] private float retreatDuration = 0.8f;

    [Header("Silk Thread")]
    [SerializeField] private float silkWidth = 0.03f;
    [SerializeField] private Color silkColor = new Color(0.9f, 0.9f, 0.9f, 0.8f);

    private SpiderState _state = SpiderState.Idle;
    private Vector2 _originPosition;
    private Vector2 _moveStartPosition;
    private Vector2 _moveEndPosition;
    private float _moveTimer;
    private float _moveDuration;
    private LineRenderer _silkThread;

    public SpiderState State => _state;

    public override void Awake()
    {
        base.Awake();
        _originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        InitializeSilkThread();
    }

    public override void Update()
    {
        base.Update();

        if (_isDying)
        {
            _silkThread.enabled = false;
            return;
        }

        float horizontalDistance = Mathf.Abs(transform.position.x - player.position.x);

        switch (_state)
        {
            case SpiderState.Idle:
                if (horizontalDistance <= detectionRange)
                {
                    _state = SpiderState.Attacking;
                    _moveStartPosition = _originPosition;
                    _moveEndPosition = new Vector2(_originPosition.x, dropTarget.position.y);
                    _moveTimer = 0f;
                    _moveDuration = dropDuration;
                    _silkThread.enabled = true;
                }
                break;

            case SpiderState.Attacking:
                _moveTimer += Time.deltaTime;
                float attackT = Mathf.Clamp01(_moveTimer / _moveDuration);
                transform.position = Vector2.Lerp(
                    _moveStartPosition, _moveEndPosition, QuarticEaseOut(attackT)
                );

                if (horizontalDistance > retreatRange)
                {
                    BeginRetreat();
                }
                break;

            case SpiderState.Retreating:
                _moveTimer += Time.deltaTime;
                float retreatT = Mathf.Clamp01(_moveTimer / _moveDuration);
                transform.position = Vector2.Lerp(
                    _moveStartPosition, _originPosition, QuarticEaseOut(retreatT)
                );

                if (retreatT >= 1f)
                {
                    _state = SpiderState.Idle;
                    transform.position = _originPosition;
                    _silkThread.enabled = false;
                }
                break;
        }

        if (_silkThread.enabled)
        {
            UpdateSilkThread();
        }
    }

    private void InitializeSilkThread()
    {
        _silkThread = gameObject.AddComponent<LineRenderer>();
        _silkThread.positionCount = 2;
        _silkThread.startWidth = silkWidth;
        _silkThread.endWidth = silkWidth;
        _silkThread.material = new Material(Shader.Find("Sprites/Default"));
        _silkThread.startColor = silkColor;
        _silkThread.endColor = silkColor;
        _silkThread.sortingLayerName = spriteRenderer.sortingLayerName;
        _silkThread.sortingOrder = spriteRenderer.sortingOrder - 1;
        _silkThread.useWorldSpace = true;
        _silkThread.enabled = false;
    }

    private void UpdateSilkThread()
    {
        _silkThread.SetPosition(0, new Vector3(_originPosition.x, _originPosition.y + 0.5f));
        _silkThread.SetPosition(1, transform.position);
    }

    private void BeginRetreat()
    {
        _state = SpiderState.Retreating;
        _moveStartPosition = transform.position;
        _moveTimer = 0f;
        _moveDuration = retreatDuration;
    }

    private static float QuarticEaseOut(float t)
    {
        float u = 1f - t;
        return 1f - u * u * u * u;
    }
}
