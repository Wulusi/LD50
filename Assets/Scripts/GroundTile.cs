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

    [SerializeField]
    private bool isDebuggingMode = false;

    [SerializeField]
    private int autoRestoreTimer;

    [SerializeField]
    GameObject DamagedTileSprite;

    private MeteorSpawner spawner;
    private bool isRoutineActive = false;
    private bool isDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        //isDebuggingMode = FindObjectOfType<GameManager>().isDebuggingEnabled();
        spawner = GetComponentInParent<MeteorSpawner>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTile()
    {
        if (groundTileHealth <= 0)
        {
            isDestroyed = true;

            if (spawner != null)
            {
                spawner.organizeTiles(this);
            }

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
        isDestroyed = false;
        if(spawner != null)
        {
            spawner.organizeTiles(this);
        }
        Debug.Log("Reset tile" + gameObject.name);
        groundTileHealth = 2;
        isRoutineActive = false;
        CheckTile();
    }
    public void DamageGroundTile()
    {
        groundTileHealth--;
        if (groundTileHealth < 2)
        {
            DamagedTileSprite.SetActive(true);
        }
        CheckTile();
    }
    private IEnumerator autoRestore()
    {
        isRoutineActive = true;
        yield return new WaitForSeconds(autoRestoreTimer);
        ResetGroundTile();
    }
    public bool isTileDestroyed()
    {
        return isDestroyed;
    }
}
