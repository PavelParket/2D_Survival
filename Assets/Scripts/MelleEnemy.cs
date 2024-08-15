using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleEnemy : Enemy
{
    private float timer;

    [SerializeField] float timeBtwAttack, attackSpeed;
    [SerializeField] int damage;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        timer += Time.fixedDeltaTime;

        if (CheckIfCanAttack())
        {

            if (timer >= timeBtwAttack)
            {
                timer = 0;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        PlayerMove.instance.Damage(damage);

        Vector2 originalPosition = transform.position;
        Vector2 playerPosition = PlayerMove.instance.transform.position;

        float percent = 0f;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = Vector2.Lerp(originalPosition, playerPosition, interpolation);

            yield return null;
        }
    }
}
