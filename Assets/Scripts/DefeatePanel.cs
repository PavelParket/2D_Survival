using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;

    private void Start()
    {
        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
        score.text = "Score: " + waveSpawner.currentWaveIndex;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
