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
    TextMeshProUGUI scoreDisplayGameOver, scoreDisplayGameOver2;

    [SerializeField]
    TextMesh scoreDisplayText, timeDisplayText; 

    [SerializeField]
    private float time;
    
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

    bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        scoreDisplayText.text = (string.Format("Score: {0}", score.ToString()));

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

        if (!isGameOver)
        {
            time += Time.deltaTime;
            timeDisplayText.text = string.Format("Time: {0}", time.ToString("F1"));
        }
    }

    public void GameOverState()
    {
        if(GameOverScreen != null)
        {
            isGameOver = true;
            Cursor.visible = true;
            scoreDisplayGameOver.SetText(string.Format("Final score: {0}", score.ToString()));
            scoreDisplayGameOver2.SetText(string.Format("Survived for {0} seconds!", time.ToString("F2")));
            GameOverScreen.SetActive(true);
        }
    }
    public void IncreaseScore()
    {
        score += 10;
        scoreDisplayText.text = string.Format("Score: {0}", score.ToString());

        if (score % 50 == 0)
        {
            meteorSpawner.decreaseSpawnCoolDown(0.025f);
            player.changeFireRate(-.025f);
        }

        if(score % 250 == 0)
        {
            meteorSpawner.decreaseSpawnCoolDown(0.05f);
            player.changeFireRate(-.05f);
        }

        if(score % 400 == 0)
        {
            meteorSpawner.increaseSpawnCount(1);
        }
    }

    private void OnDestroy()
    {
        player.onPlayerKilled -= GameOverState;
    }
}
