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
    GameObject autoDestroyTrigger;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private bool isDebuggingMode = false;

    [SerializeField]
    private int autoRestoreTimer;

    [SerializeField]
    GameObject DamagedTileSprite;

    [SerializeField]
    CameraShake cameraShake;

    [SerializeField]
    private AudioSource groundHit;

    private MeteorSpawner spawner;
    private bool isRoutineActive = false;
    private bool isDestroyed;
    private bool startAutoDestroy;
    // Start is called before the first frame update
    void Start()
    {
        //isDebuggingMode = FindObjectOfType<GameManager>().isDebuggingEnabled();
        spawner = GetComponentInParent<MeteorSpawner>();
        cameraShake = FindObjectOfType<CameraShake>();
        autoDestroyTrigger.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void enableAutoDestroy()
    {
        autoDestroyTrigger.SetActive(true);
    }

    public void startAutoDestroyTime()
    {
        startAutoDestroy = true;
        StartCoroutine(autoDestroy());
    }

    public void checkIfPlayerPresent()
    {
        if(startAutoDestroy)
        {
            StartCoroutine(autoDestroy());
        }
    }

    public void disableAutoDestroy()
    {
        startAutoDestroy = false;
    }
    private IEnumerator autoDestroy()
    {
        float elapsedtime = 0;
        Vector3 originalPos = this.transform.localScale;

        while (startAutoDestroy)
        {
            elapsedtime += Time.deltaTime;

            float x = Random.Range(0.7f, 1.1f);
            float y = Random.Range(0.9f, 1.1f);

            //Debug.LogWarning("elapsed time is " + elapsedtime + this.transform.name);

            this.transform.localScale = new Vector3(originalPos.x, y, originalPos.z);

            if (elapsedtime > 5)
            {
                DamageGroundTile();
                break;
            }
            yield return null;
        }
        this.transform.localScale = originalPos;
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
        groundHit.Play();

        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(0.25f, 0.1f));
        }

        if (groundTileHealth < 2)
        {
            DamagedTileSprite.SetActive(true);
        }
        CheckTile();
        checkIfPlayerPresent();
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
