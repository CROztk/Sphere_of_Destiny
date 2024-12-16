using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // see if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = new Vector2(knockback.x * Mathf.Sign(transform.localScale.x), knockback.y);
            damageable.Hit(attackDamage, deliveredKnockback);
        }
    }
}
