using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] float speed;
    [SerializeField] Transform target;

    Animator animator;

    public static CameraFollow instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!target) return;

        transform.position = Vector3.Lerp(transform.position,
            new Vector3(
                Mathf.Clamp(target.position.x, minX, maxX),
                Mathf.Clamp(target.position.y, minY, maxY),
                -10),
            speed * Time.fixedDeltaTime);
    }

    public void ShakeCamera()
    {
        animator.Play("ShakeCamera");
    }
}
