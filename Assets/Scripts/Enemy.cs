using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    bool isDeath = false;
    bool canAttack = false;

    Vector3 addRandPosToGo;

    [SerializeField] int health, minCoins, maxCoins;
    [SerializeField] float attackDistance, runOutDistance, speed;
    [SerializeField] GameObject hitParticle;

    protected PlayerMove player;

    public virtual void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = PlayerMove.instance;

        StartCoroutine(SetRandomPos());
        EnemyOrderInLayerManager.instance.Add(spriteRenderer);
    }

    protected void OnDestroy()
    {
        EnemyOrderInLayerManager.instance.Remove(spriteRenderer);
    }

    public virtual void FixedUpdate()
    {
        if (isDeath || !player) return;

        if (Vector2.Distance(transform.position, player.transform.position) > attackDistance)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, speed * Time.fixedDeltaTime);
            rigidbody2d.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, speed * Time.fixedDeltaTime));
            animator.SetBool("Run", true);
            canAttack = false;
        }
        else if (Vector2.Distance(transform.position, player.transform.position) < runOutDistance)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, -speed * Time.fixedDeltaTime);
            rigidbody2d.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, -speed * Time.fixedDeltaTime));
            animator.SetBool("Run", true);
            canAttack = false;
        }
        else
        {
            animator.SetBool("Run", false);
            canAttack = true;
        }

        ScaleEnemy();
    }

    protected void ScaleEnemy()
    {
        if (transform.position.x > player.transform.position.x)
            spriteRenderer.flipX = true;
        else if (transform.position.x < player.transform.position.x)
            spriteRenderer.flipX = false;
    }

    public void Damage(int damage)
    {
        if (isDeath) return;

        health -= damage;

        Instantiate(hitParticle, transform.position, Quaternion.identity);

        if (health <= 0)
            DeathAnimation();
    }

    protected void DeathAnimation()
    {
        isDeath = true;
        player.AddCoins(Random.Range(minCoins, maxCoins));

        if (PlayerPrefs.GetInt("Position2") == 1)
            player.Heal(1);

        animator.SetTrigger("Death");
    }

    public virtual IEnumerator DeathEnemy()
    {
        float transparency = spriteRenderer.color.a;
        while (spriteRenderer.color.a != 0)
        {
            spriteRenderer.color = new Color(255f, 0f, 0f, transparency -= 0.5f);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    public virtual bool CheckIfCanAttack()
    {
        return canAttack && !isDeath;
    }

    IEnumerator SetRandomPos()
    {
        addRandPosToGo = new Vector3(Random.Range(-attackDistance + 0.1f, attackDistance - 0.1f), Random.Range(-attackDistance + 0.1f, attackDistance - 0.1f));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(SetRandomPos());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Враг столкнулся со стеной!");
        }
    }
}
