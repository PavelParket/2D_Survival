using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    private float timer;

    [SerializeField] float timeBtwAttack;
    [SerializeField] Transform shootPosition;
    [SerializeField] GameObject bullet;

    public override void Start()
    {
        base.Start();

        timer = timeBtwAttack;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        timer += Time.fixedDeltaTime;

        if (CheckIfCanAttack() && player)
        {
            if (timer >= timeBtwAttack)
            {
                timer = 0;
                Attack();
            }
        }

    }

    void Attack()
    {
        Vector2 direction = player.transform.position - shootPosition.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shootPosition.rotation = rotation;

        Instantiate(bullet, shootPosition.position, shootPosition.rotation);
    }

    public override IEnumerator DeathEnemy()
    {
        return base.DeathEnemy();
    }
}
