using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public Enemy[] enemies;
        public int count;
        public float timeBtwSpawn;
    }

    [SerializeField] float timeBtwWaves, currentTimeBtwWaves;
    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] TextMeshProUGUI waveText;

    [HideInInspector] public int currentWaveIndex;

    private bool isSpawnFinished = false;
    private bool isFreeTime = false;

    private Wave currentWave;

    private Transform player;

    private void Start()
    {
        player = PlayerMove.instance.transform;
        currentTimeBtwWaves = timeBtwWaves;
        UpdateWaveText();

        StartCoroutine(SpawnWave(currentWaveIndex));
    }

    private void Update()
    {
        UpdateWaveText();

        if (isSpawnFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawnFinished = false;

            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(CallNextWave(currentWaveIndex));
            }
        }
    }

    IEnumerator CallNextWave(int waveIndex)
    {
        currentTimeBtwWaves = timeBtwWaves;
        isFreeTime = true;
        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        StartCoroutine(SpawnWave(waveIndex));
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        currentWave = waves[waveIndex];

        for (int i = 0; i < currentWave.count; i++)
        {
            if (player == null) yield break;

            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);

            if (i == currentWave.count - 1) isSpawnFinished = true;
            else isSpawnFinished = false;

            yield return new WaitForSeconds(currentWave.timeBtwSpawn);
        }
    }

    void UpdateWaveText()
    {
        if (isFreeTime) waveText.text = "Next wave: " + (int)(currentTimeBtwWaves -= Time.deltaTime);
        else waveText.text = "Wave: " + (currentWaveIndex + 1);
    }
}
