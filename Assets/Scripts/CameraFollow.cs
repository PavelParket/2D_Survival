using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] Transform target;
    [SerializeField] float speed;

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
}
