using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float accelerationFactor = 10.0f;
    public float maxSpeed = 5.0f;
    public float turnFactor = 3.5f;
    public float driftFactor = 0.95f;
    public Rigidbody2D vehicleRb;

    float accelerationInput = 0.0f;
    float steeringInput = 0.0f;
    float rotationAngle = 0.0f;
    float dragFactor = 3.0f;
    Vector2 lastInput = Vector2.zero;
    bool isBeingDamaged = false;
    float damageDuration = 0.0f;
    DynamicObstacleDamageType damageType;
    private bool stopUpdatingMotion = false;

    private void Start()
    {
        //GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        if (stopUpdatingMotion)
            return;

        if (isBeingDamaged)
        {
            switch (damageType)
            {
                case DynamicObstacleDamageType.TiresBlown:
                    break;
                case DynamicObstacleDamageType.Slow:
                    break;
                case DynamicObstacleDamageType.SpinNoSteering:
                    // Janky - maybe apply impulses instead in FixedUpdate
                    transform.Rotate(0.0f, 0.0f, 360.0f * Time.deltaTime);

                    break;
            }

            if (damageDuration > 0)
            {
                damageDuration -= Time.deltaTime;
            }
            else
            {
                damageDuration = 0.0f;
                isBeingDamaged = false;
                //GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    void FixedUpdate()
    {
        if (stopUpdatingMotion)
            return;

        float dragDamageFactor = 1.0f;
        float accelerationDamageFactor = 1.0f;
        float driftDamageFactor = 1.0f;
        bool applyDrag = true;
        bool applyDrift = true;
        bool applySeering = true;

        if (isBeingDamaged)
        {
            switch (damageType)
            {
                case DynamicObstacleDamageType.TiresBlown:
                    applyDrag = false;
                    applyDrift = false;

                    break;
                case DynamicObstacleDamageType.Slow:
                    accelerationDamageFactor = 0.1f;

                    break;
                case DynamicObstacleDamageType.SpinNoSteering:
                    dragDamageFactor = 0.0f;
                    applySeering = false;

                    break;
            }
        }

        // Apply Drag
        if (applyDrag)
        {
            vehicleRb.drag = CalculateDrag() * dragDamageFactor;
        } else
        {
            vehicleRb.drag = 0.0f;
        }

        // Apply Acceleration
        vehicleRb.AddForce(CalculateAccelerationForce() * accelerationDamageFactor, ForceMode2D.Force);

        // Apply Drift
        if (applyDrift)
        {
            vehicleRb.velocity = CalculateDrift() * driftDamageFactor;
        }
        
        // Apply Steering
        if (applySeering)
        {
            vehicleRb.MoveRotation(CalculateSteering());
        }
    }

    private float CalculateDrag()
    {
        if (accelerationInput == 0)
        {
            return Mathf.Lerp(
                vehicleRb.drag, dragFactor,
                Time.fixedDeltaTime * dragFactor
            );
        }
        else
        {
            return 0;
        }
    }

    private float calculatedLateralVelocity
    {
        get
        {
            return Vector2.Dot(transform.right, vehicleRb.velocity);
        }
    }

    private Vector2 CalculateAccelerationForce()
    {
        // Play acceleration sound if accelerating in forward or reverse
        if (accelerationInput != 0)
        {
            AudioManager.Instance.PlayAccelerate();
        }
        else
        {
            AudioManager.Instance.StopPlayingAccelerate();
        }

        // First check max speed
        float forwardVelocity = Vector2.Dot(transform.up, vehicleRb.velocity);
        if (forwardVelocity > maxSpeed && accelerationInput > 0)
        {
            return Vector2.zero;
        }
        else
        {
            return transform.up * accelerationInput * accelerationFactor;
        }
    }

    private Vector2 CalculateDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(vehicleRb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(vehicleRb.velocity, transform.right);
        return forwardVelocity + (rightVelocity * driftFactor);
    }

    private float CalculateSteering()
    {
        float steeringMinSpeedFactor = Mathf.Clamp01(vehicleRb.velocity.magnitude / 2);
        rotationAngle -= steeringInput * turnFactor * steeringMinSpeedFactor;
        return rotationAngle;
    }

    public bool TireSkid(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = calculatedLateralVelocity;
        isBraking = false;

        float forwardVelocity = Vector2.Dot(transform.up, vehicleRb.velocity);

        if (accelerationFactor < 0 && forwardVelocity > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(lateralVelocity) > 3.5f)
        {
            return true;
        }

        return false;
    }

    public void SetInput(Vector2 input)
    {
        // If the vehicle is being damaged, stop updating the input
        if (isBeingDamaged)
        {
            switch (damageType)
            {
                case DynamicObstacleDamageType.TiresBlown:
                    lastInput = input;
                    steeringInput = input.x;
                    accelerationInput = input.y;

                    break;
                case DynamicObstacleDamageType.Slow:
                    lastInput = input;
                    steeringInput = input.x;
                    accelerationInput = input.y;

                    break;
                case DynamicObstacleDamageType.SpinNoSteering:
                    steeringInput = lastInput.x;
                    accelerationInput = lastInput.y;

                    break;
            }
        }
        else
        {
            lastInput = input;
            steeringInput = input.x;
            accelerationInput = input.y;
        }
    }

    public void StopVehicle()
    {
        Debug.Log("STOPPING VEHICLE");
        accelerationInput = 0.0f;
        steeringInput = 0.0f;
        vehicleRb.velocity = Vector2.zero;
        vehicleRb.angularVelocity = 0.0f;
        vehicleRb.freezeRotation = true;
        stopUpdatingMotion = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DynamicObstacle"))
        {
            IDynamicObstacle obstacle = (IDynamicObstacle)collision.gameObject.GetComponent(typeof(IDynamicObstacle));
            InflictDynamicObstacleDamage(obstacle.DamageType, obstacle.DamageDuration);
        }
    }

    private void InflictDynamicObstacleDamage(DynamicObstacleDamageType type, float duration)
    {
        isBeingDamaged = true;
        damageType = type;
        damageDuration = duration;

        if (damageType == DynamicObstacleDamageType.TiresBlown)
        {
            // Enable sparks for Tire Strip (needs work on material)
        }
    }
}
