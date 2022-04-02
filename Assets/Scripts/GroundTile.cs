using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundTile : MonoBehaviour
{

    [SerializeField]
    private float groundTileHealth;

    [SerializeField]
    private Collider2D blocker;

    [SerializeField]
    private SpriteRenderer sprite;

    private bool isDebuggingMode;

    [SerializeField]
    private int autoRestoreTimer;

    [SerializeField]
    GameObject DamagedTileSprite;

    private bool isRoutineActive = false;
    // Start is called before the first frame update
    void Start()
    {
        isDebuggingMode = FindObjectOfType<GameManager>().isDebuggingEnabled();
    }
    // Update is called once per frame
    void Update()
    {
        if (groundTileHealth <= 0)
        {
            blocker.enabled = false;
            sprite.enabled = false;
            DamagedTileSprite.SetActive(false);

            if (isDebuggingMode)
            {
                if (!isRoutineActive)
                {
                    StartCoroutine(autoRestore());
                }
            }
        }
        else
        {
            blocker.enabled = true;
            sprite.enabled = true;
        }
    }

    public void ResetGroundTile()
    {
        DamagedTileSprite.SetActive(false);
        Debug.Log("Reset tile" + gameObject.name);
        groundTileHealth = 2;
        isRoutineActive = false;
    }

    public void DamageGroundTile()
    {
        groundTileHealth--;

        if(groundTileHealth < 2)
        {
            DamagedTileSprite.SetActive(true);
        }
    }

    private IEnumerator autoRestore()
    {
        isRoutineActive = true;
        yield return new WaitForSeconds(autoRestoreTimer);
        ResetGroundTile();
    }

    
}
