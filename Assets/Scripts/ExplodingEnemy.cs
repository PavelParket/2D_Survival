using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    [SerializeField] float radius;
    [SerializeField] int damage;
    [SerializeField] LayerMask whatIsPlayer;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (CheckIfCanAttack() && player)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] detectedObject = Physics2D.OverlapCircleAll(transform.position, radius, whatIsPlayer);

        foreach (Collider2D c in detectedObject)
        {
            c?.GetComponent<PlayerMove>()?.Damage(damage);
        }

        DeathAnimation();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override IEnumerator DeathEnemy()
    {
        return base.DeathEnemy();
    }
}
