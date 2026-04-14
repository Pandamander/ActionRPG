using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSerpentBossController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SeaSerpentBossMotor motor;
    [SerializeField] private SeaSerpentBossAttackController attacks;
    [SerializeField] private SeaSerpentBossHealth health;

    [Header("Arena Anchors")]
    [SerializeField] private Transform leftAnchor;
    [SerializeField] private Transform rightAnchor;

    [Header("Timing")]
    [SerializeField] private float idleDuration = 2f;

    private ISeaSerpentBossState currentState;

    public SeaSerpentBossMotor Motor => motor;
    public SeaSerpentBossAttackController Attacks => attacks;
    public SeaSerpentBossHealth Health => health;

    public Transform LeftAnchor => leftAnchor;
    public Transform RightAnchor => rightAnchor;
    public float IdleDuration => idleDuration;

    private void Awake()
    {
        if (motor == null) motor = GetComponent<SeaSerpentBossMotor>();
        if (attacks == null) attacks = GetComponent<SeaSerpentBossAttackController>();
        if (health == null) health = GetComponent<SeaSerpentBossHealth>();
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDied += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDied -= HandleDeath;
        }
    }

    private void Start()
    {
        ChangeState(new SeaSerpentBossIdleState(this));
    }

    private void Update()
    {
        motor.Tick();
        attacks.Tick();
        currentState?.Tick();
    }

    public void ChangeState(ISeaSerpentBossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void HandleDeath()
    {
        ChangeState(new SeaSerpentBossDeadState(this));
    }

    public Vector3 GetRandomSideTarget()
    {
        if (Random.value < 0.5f)
        {
            return leftAnchor.position;
        }

        return rightAnchor.position;
    }
}

public class SeaSerpentBossIdleState : ISeaSerpentBossState
{
    private readonly SeaSerpentBossController boss;
    private float timer;

    public SeaSerpentBossIdleState(SeaSerpentBossController boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        timer = boss.IdleDuration;
        boss.Motor.StartIdle();

        Debug.Log("Boss: Idle");
    }

    public void Tick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Vector3 target = boss.GetRandomSideTarget();
            boss.ChangeState(new SeaSerpentBossMoveState(boss, target));
        }
    }

    public void Exit()
    {
        boss.Motor.StopIdle();
    }
}

public class SeaSerpentBossMoveState : ISeaSerpentBossState
{
    private readonly SeaSerpentBossController boss;
    private readonly Vector3 target;

    public SeaSerpentBossMoveState(SeaSerpentBossController boss, Vector3 target)
    {
        this.boss = boss;
        this.target = target;
    }

    public void Enter()
    {
        boss.Motor.MoveTo(target);
        Debug.Log("Boss: Move");
    }

    public void Tick()
    {
        if (boss.Motor.IsAtTarget())
        {
            boss.ChangeState(new SeaSerpentBossAttackState(boss));
        }
    }

    public void Exit()
    {
    }
}

public class SeaSerpentBossAttackState : ISeaSerpentBossState
{
    private readonly SeaSerpentBossController boss;

    public SeaSerpentBossAttackState(SeaSerpentBossController boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        string attackName = Random.value < 0.5f ? "AttackA" : "AttackB";
        boss.Attacks.StartAttack(attackName);

        Debug.Log("Boss: Attack");
    }

    public void Tick()
    {
        if (boss.Attacks.IsAttackFinished())
        {
            boss.ChangeState(new SeaSerpentBossIdleState(boss));
        }
    }

    public void Exit()
    {
    }
}

public class SeaSerpentBossDeadState : ISeaSerpentBossState
{
    private readonly SeaSerpentBossController boss;

    public SeaSerpentBossDeadState(SeaSerpentBossController boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Motor.StopIdle();
        Debug.Log("Boss: Dead");

        // Later:
        // play death animation
        // disable hurtbox
        // notify encounter controller
        // drop reward, end fight, etc.
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}