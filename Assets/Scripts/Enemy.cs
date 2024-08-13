using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    bool isDeath = false;

    [SerializeField] int health;
    [SerializeField] float attackDistance, runOutDistance, speed;

    PlayerMove player;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = PlayerMove.instance;
    }

    private void FixedUpdate()
    {
        if (isDeath) return;

        if (Vector2.Distance(transform.position, player.transform.position) > attackDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            animator.SetBool("Run", true);
        }
        else if (Vector2.Distance(transform.position, player.transform.position) < runOutDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.fixedDeltaTime);
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        ScaleEnemy();
    }

    void ScaleEnemy()
    {
        if (transform.position.x > player.transform.position.x) spriteRenderer.flipX = true;
        else if (transform.position.x < player.transform.position.x) spriteRenderer.flipX = false;
    }

    public void Damage(int damage)
    {
        if (isDeath) return;

        health -= damage;

        if (health <= 0) DeathAnimation();
    }

    void DeathAnimation()
    {
        isDeath = true;
        animator.SetTrigger("Death");
    }

    public IEnumerator DeathEnemy()
    {
        float transparency = spriteRenderer.color.a;
        while (spriteRenderer.color.a != 0)
        {
            spriteRenderer.color = new Color(255f, 0f, 0f, transparency -= 0.5f);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
