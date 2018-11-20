using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour {
    [Header("UI")]
    [SerializeField] int currentScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI lifeText;

    [Header("Spawn")]
    [SerializeField] GameObject[] comets;
    [SerializeField] int waveCounter = 0;
    [SerializeField] int cometCountIncrease = 1;
    [SerializeField] float spawnSpeedIncrease = 0.1f;
    [SerializeField] Vector2 spawnValues;
    [SerializeField] int cometCount;
    [SerializeField] float spawnWait;
    [SerializeField] float startWait;
    [SerializeField] float waveWait;

    private void Awake()
    {
        int levelManagerNumber = FindObjectsOfType<LevelManager>().Length;
        if (levelManagerNumber > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start () 
    {
        GetComponent<AudioSource>().Play();
        UpdateScore();
        StartCoroutine(SpawnWaves());
	}

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while(true)
        {
            for (int i = 0; i < cometCount; i++)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y));
                Instantiate(comets[Random.Range(0, comets.Length)], spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait - (waveCounter * spawnSpeedIncrease));
            }
            yield return new WaitForSeconds(waveWait);
            waveCounter++;
        }
    }

    public void ManageLives (int lives)
    {
        lifeText.text = "lives: " + lives.ToString();
    }

    public void AddPoints(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text =currentScore.ToString();
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}
