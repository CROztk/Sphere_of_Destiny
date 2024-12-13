using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate= 0.05f;
    public DetectionZone attackZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections; 
    Animator animator;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkableDirection;
    public Vector2 walkableDirectionVector = Vector2.right;
    
    public WalkableDirection walkableDirection
    {
        get { return _walkableDirection; }
        set
        {
            if (_walkableDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkableDirectionVector  = Vector2.right;
                }else if (value == WalkableDirection.Left)
                {
                    walkableDirectionVector  = Vector2.left;
                }
            }
            _walkableDirection = value;
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    public bool _hasTarget = false;

    public bool HasTarget {
        get { return _hasTarget; } private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if(CanMove) 
            rb.velocity = new Vector2(walkSpeed * walkableDirectionVector.x, rb.velocity.y);
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x,0,walkStopRate), rb.velocity.y);
        }
        
    }

    private void FlipDirection()
    {
        if (walkableDirection == WalkableDirection.Right)
        {
            walkableDirection = WalkableDirection.Left;
        } else if (walkableDirection == WalkableDirection.Left)
        {
            walkableDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set tot legal values of right or left");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D" + collision.collider.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnCollisionEnter2D" + collision.collider.gameObject.name);
            Destroy(gameObject);
        }
    }
}
