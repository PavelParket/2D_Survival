using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    private int maxHealth;
    private float shootTimer, dashTimer;
    private bool isDashing = false, isInvincible = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private Vector2 moveVelocity;

    [SerializeField] int health;
    [SerializeField] float moveSpeed, timeBtwShoot, dashForce, timeBtwDash, dashTime;
    [SerializeField] GameObject bullet, hitParticle, defeatePanel;
    [SerializeField] Transform shootPosition;
    [SerializeField] Sprite[] spriteMuzzleFlash;
    [SerializeField] SpriteRenderer muzzleFlashSpriteRenderer;
    [SerializeField] Slider healthSlider, dashSlider;
    [SerializeField] ParticleSystem footParticle;

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
        dashTimer = timeBtwDash;
        maxHealth = health;

        UpdateHealth();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer >= timeBtwShoot)
        {
            Shoot();
            shootTimer = 0;
        }

        dashTimer += Time.deltaTime;
        dashSlider.value = dashTimer / timeBtwDash;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashTimer >= timeBtwDash)
            {
                dashTimer = 0;
                ActivateDash();
            }
        }
    }

    private void FixedUpdate()
    {
        Move();

        if (isDashing) Dash();
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("Run", true);
            footParticle.Pause();
            footParticle.Play();
            var emission = footParticle.emission;
            emission.rateOverTime = 10;
        }
        else
        {
            animator.SetBool("Run", false);
            var emission = footParticle.emission;
            emission.rateOverTime = 0;
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

        StartCoroutine(SetMuzzleFlash());
    }

    IEnumerator SetMuzzleFlash()
    {
        muzzleFlashSpriteRenderer.enabled = true;
        muzzleFlashSpriteRenderer.sprite = spriteMuzzleFlash[Random.Range(0, spriteMuzzleFlash.Length)];

        yield return new WaitForSeconds(0.1f);

        muzzleFlashSpriteRenderer.enabled = false;
    }

    public void Damage(int damage)
    {
        if (isInvincible) return;

        health -= damage;

        Instantiate(hitParticle, transform.position, Quaternion.identity);

        CameraFollow.instance.ShakeCamera();

        UpdateHealth();

        if (health <= 0)
        {
            defeatePanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    void ActivateDash()
    {
        isDashing = true;
        isInvincible = true;

        Invoke(nameof(DeActivateDash), dashTime);
    }

    void DeActivateDash()
    {
        isDashing = false;
        isInvincible = false;
    }

    void Dash()
    {
        rigidbody2d.AddForce(moveVelocity * Time.fixedDeltaTime * dashForce * 1000);
    }

    void UpdateHealth()
    {
        healthSlider.value = (float)health / maxHealth;
    }
}
