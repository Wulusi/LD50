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
    TextMeshProUGUI scoreDisplay, scoreDisplayGameOVer;

    [SerializeField]
    private float time;

    [SerializeField]
    TextMeshProUGUI timerDisplay, timerDisplayDisplayGameOVer;
    
    [SerializeField]
    private bool isDebug;

    [SerializeField]
    GameObject PlayerPrefab;

    [SerializeField]
    GameObject GameOverScreen;

    [SerializeField]
    MeteorSpawner meteorSpawner;

    [SerializeField]
    private float multiplier = 1;

    [SerializeField]
    private CharacterController player;

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.SetText(string.Format("Score: {0}", score.ToString()));

        player.onPlayerKilled += GameOverState;

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

        //timerDisplay.SetText(string.Format("Time: {0}"), time.ToString("F2")));
    }

    public void GameOverState()
    {
        if(GameOverScreen != null)
        {
            Time.timeScale = 0;

            scoreDisplayGameOVer.SetText(string.Format("Your score was: {0}, and you survived for {1} seconds!",
                score.ToString(), time.ToString("F2")));
            GameOverScreen.SetActive(true);
        }
    }
    public void IncreaseScore()
    {
        score += 10;
        scoreDisplay.SetText(string.Format("Score: {0}", score.ToString()));

        if (score % 50 == 0)
        {
            meteorSpawner.decreaseSpawnCoolDown(0.025f);
        }

        if(score % 250 == 0)
        {
            meteorSpawner.decreaseSpawnCoolDown(0.05f);
        }
    }

    private void OnDestroy()
    {
        player.onPlayerKilled -= GameOverState;
    }
}
