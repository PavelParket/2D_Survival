using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    private float shootTimer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private Vector2 moveVelocity;

    [SerializeField] float moveSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPosition;
    [SerializeField] float timeBtwShoot;
    [SerializeField] TextMeshProUGUI time;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootTimer = timeBtwShoot;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer >= timeBtwShoot)
        {
            Shoot();
            shootTimer = 0;
        }

        time.text = Time.time.ToString();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        ScalePlayer(moveInput.x);
        moveVelocity = moveInput.normalized * moveSpeed;
        rigidbody2d.MovePosition(rigidbody2d.position + moveVelocity * Time.fixedDeltaTime);
    }

    void ScalePlayer(float x)
    {
        if (x == 1) spriteRenderer.flipX = false;
        else if (x == -1) spriteRenderer.flipX = true;
    }

    void Shoot()
    {
        Instantiate(bullet, shootPosition.position, shootPosition.rotation);
    }
}
