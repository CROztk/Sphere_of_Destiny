using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(TouchingDirections)), RequireComponent(typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    public float reviveTime = 3f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

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

    public bool IsAlive { get
    {
        return animator.GetBool(AnimationStrings.isAlive);
    } }

    

    Rigidbody2D rb;
    Animator animator;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }


    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * Speed, rb.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if(!IsAlive)
        {
            reviveTime -= Time.fixedDeltaTime;
            if(reviveTime <= 0.1f)
            {
                reviveTime = 3f;
                // Reload the scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        if(IsAlive)
        {
            IsMoving = moveInput.x != 0;
            SetFacingDirection();
        }else
        {
            IsMoving = false;
        }
        
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
            animator.SetTrigger(AnimationStrings.jumpTrigger);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
