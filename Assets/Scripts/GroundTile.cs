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
            blocker.isTrigger = true;
            sprite.enabled = false;
            DamagedTileSprite.SetActive(false);

            if (isDebuggingMode)
            {
                StartCoroutine(autoRestore());
            }
        }
        else
        {
            blocker.isTrigger = false;
            sprite.enabled = true;
        }
    }

    public void ResetGroundTile()
    {
        groundTileHealth = 2;
        DamagedTileSprite.SetActive(false);
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
        yield return new WaitForSeconds(autoRestoreTimer);
        ResetGroundTile();
    }

    
}
