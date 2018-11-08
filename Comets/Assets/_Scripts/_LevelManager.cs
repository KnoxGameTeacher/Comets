using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _LevelManager : MonoBehaviour {

    [SerializeField] int currentScore = 0;
    [SerializeField] int pointsPerBlock = 100;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        int levelManagerNumber = FindObjectsOfType<_LevelManager>().Length;
        if (levelManagerNumber > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    // Use this for initialization
    void Start () 
    {
        GetComponent<AudioSource>().Play();
        UpdateScore();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void AddToScore()
    {
        currentScore += pointsPerBlock;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = currentScore.ToString();
    }

    public void ResetGame()
    {
        //currentScore = 0;
        //UpdateScore();
        Destroy(gameObject);
    }

}
