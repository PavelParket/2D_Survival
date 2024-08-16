using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrderInLayerManager : MonoBehaviour
{
    public static EnemyOrderInLayerManager instance;

    float[] posYs;

    SpriteRenderer[] spriteRenderers;

    List<SpriteRenderer> enemiesSpriteRenderer = new List<SpriteRenderer>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(1);

        int n = enemiesSpriteRenderer.Count;

        if (n == 0) yield break;

        posYs = new float[n];
        spriteRenderers = new SpriteRenderer[n];

        for (int i = 0; i < n; i++)
        {
            posYs[i] = enemiesSpriteRenderer[i].transform.position.y;
            spriteRenderers[i] = enemiesSpriteRenderer[i];
        }

        Array.Sort(posYs, spriteRenderers);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder = -i;
        }

        StartCoroutine(Check());
    }

    public void Add(SpriteRenderer spriteRenderer)
    {
        enemiesSpriteRenderer.Add(spriteRenderer);
    }

    public void Remove(SpriteRenderer spriteRenderer)
    {
        enemiesSpriteRenderer.Remove(spriteRenderer);
    }
}
