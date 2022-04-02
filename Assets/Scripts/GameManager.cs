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

    // Start is called before the first frame update
    void Start()
    {
        
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

        timerDisplay.SetText(string.Format("Time:{0}", time.ToString("F2")));
    }
}
