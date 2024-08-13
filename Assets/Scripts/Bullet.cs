using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float deathTime;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Death), deathTime);
    }

    // Update is called once per frame
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

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
