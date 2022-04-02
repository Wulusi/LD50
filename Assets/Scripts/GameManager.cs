using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;

    [SerializeField]
    TextMeshProUGUI scoreDisplay;

    [SerializeField]
    private float time;

    [SerializeField]
    TextMeshProUGUI timerDisplay;
    
    [SerializeField]
    private bool isDebug;

    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    GameObject GameOverScreen;

    [SerializeField]
    MeteorSpawner meteorSpawner;
    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.SetText(string.Format("Score: {0}", score.ToString()));

        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(false);
        }
    }

    public bool isDebuggingEnabled()
    {
        return isDebug;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDebug && Input.GetKeyDown(KeyCode.M))
        {
            Instantiate(PlayerPrefab, new Vector3(0, -2.83f, 0), Quaternion.identity);
        }

        if (isDebug && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        time = Time.time;

        timerDisplay.SetText(string.Format("Time: {0}", time.ToString("F2")));
    }

    public void GameOverState()
    {
        if(GameOverScreen != null)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
        }
    }
    public void IncreaseScore()
    {
        score += 10;
        scoreDisplay.SetText(string.Format("Score: {0}", score.ToString()));
    }
}
