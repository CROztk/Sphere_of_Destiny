using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    public float Speed {
        
        get
        {
            if (CanMove)
            {
                if ((IsMoving && !touchingDirections.IsOnWall&&CanMove))
                {
                    if (touchingDirections.IsGrounded)
                    {
                        return IsRunning ? runSpeed : walkSpeed;
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else{ return 0;}
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
    {
        return _isMoving;
    } private set
    {
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving, value);
    } }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning { get 
    {
        return _isRunning;
    } private set
    {
        _isRunning = value;
        animator.SetBool(AnimationStrings.isRunning, value);
    } }

    public bool CanMove { get
    {
        return animator.GetBool(AnimationStrings.canMove);
    } }
    Rigidbody2D rb;
    Animator animator;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * Speed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection();
        
    }

    private void SetFacingDirection()
    {
        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO Check if character is alive
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            animator.SetTrigger(AnimationStrings.jump);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.Attack);
        }
    }
}
