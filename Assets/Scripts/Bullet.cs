using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float deathTime;
    [SerializeField] int damage;
    [SerializeField] Type type;

    [System.Serializable]
    public enum Type
    {
        Player, Enemy
    }

    void Start()
    {
        Invoke(nameof(Death), deathTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Death();
        }

        if (collision.gameObject.tag == "Enemy" && type == Type.Player)
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
            Death();
        }

        if (collision.gameObject.tag == "Player" && type == Type.Enemy)
        {
            collision.gameObject.GetComponent<PlayerMove>().Damage(damage);
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
