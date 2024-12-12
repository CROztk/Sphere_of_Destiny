using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(CapsuleCollider2D)), RequireComponent(typeof(Animator))]
public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    CapsuleCollider2D col;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded { get
    {
        return _isGrounded;
    } private set
    {
        _isGrounded = value;
        animator.SetBool(AnimationStrings.isGrounded, value);
    } }

    [SerializeField]
    private bool _isOnwall = true;

    public bool IsOnWall { get
    {
        return _isOnwall;
    } private set
    {
        _isOnwall = value;
        animator.SetBool(AnimationStrings.isOnWall, value);
    } }

    [SerializeField]
    private bool _isOnCeiling = true;
    private Vector2 wallCheckDirection => new Vector2(Mathf.Sign(transform.localScale.x), 0);

    public bool IsOnCeiling { get
    {
        return _isOnCeiling;
    } private set
    {
        _isOnCeiling = value;
        animator.SetBool(AnimationStrings.isOnCeiling, value);
    } }

    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

}
