using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints; 
    public float waypointReachedDistance=0.1f;
    public float followRange = 40f;
    public bool isClever = false;
    
    
    Animator animator; 
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    private Transform target;
    int waypointNum = 0;
    private bool lockedOnTarget = false;
    
    public bool _hasTarget = false;
    
    public bool HasTarget {
        get { return _hasTarget; } private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
            if (value && isClever)
            {
                target = biteDetectionZone.detectedColliders[0].transform;
                lockedOnTarget = true;
                flightSpeed = 4f;
            }
        }
    }
    
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }


    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive && CanMove && !lockedOnTarget)
        {
            Flight();
        }else if (lockedOnTarget)
        {
            ChaseTarget();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void ChaseTarget()
    {
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(target.position, transform.position);
        if(distance < 1f)
        {
            rb.velocity = Vector2.zero;
        }else
        {
            rb.velocity = directionToTarget * flightSpeed;
        }
        UpdateDirection();
        if (distance > followRange)
        {
            lockedOnTarget = false;
        }
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
