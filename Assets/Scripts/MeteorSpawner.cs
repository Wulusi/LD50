using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> meteorSpawns = new List<GameObject>();

    [SerializeField]
    List<GroundTile> destroyedTiles = new List<GroundTile>();

    [SerializeField]
    private int spawnDistance;

    private bool canSpawn;

    [SerializeField]
    private GameObject meteor;

    [SerializeField]
    private float spawnCoolDown;
    private float timeStamp = 0;

    [SerializeField]
    private float normalSpawnChance, extraSpawnChance;

    [SerializeField]
    private int numberOfSpawns = 2;

    [SerializeField]
    private AudioSource meteorDestroySound, BonusSound;

    private int level;
    // Start is called before the first frame update
    void Start()
    {
        meteorSpawns.Clear();
        destroyedTiles.Clear();

        int numOfChildren = this.transform.childCount;

        for (int i = 0; i < numOfChildren; i++)
        {
            meteorSpawns.Add(this.transform.GetChild(i).gameObject);
        }
    }
    void Update()
    {
        if (timeStamp < Time.time)
        {
            timeStamp = Time.time + spawnCoolDown;
            SpawnMeteorsSequence(numberOfSpawns);
        }
    }
    void SpawnMeteorsSequence(int spawnAmount)
    {

        for (int i = 0; i < spawnAmount; i++)
        {
            canSpawn = false;

            int randomSpawn = Random.Range(0, meteorSpawns.Count);

            int spawnOffset = Random.Range(0, 5);

            Vector3 spawnlocation = meteorSpawns[randomSpawn].transform.position + new Vector3(0, spawnDistance + spawnOffset);

            if (meteor != null)
            {
                GameObject fallingMeteor = Instantiate(meteor, spawnlocation, Quaternion.identity);

                if (RollDice())
                {
                    fallingMeteor.GetComponent<Meteor>().setBonus(1);
                }
                else if (RollDice2())
                {
                    fallingMeteor.GetComponent<Meteor>().setBonus(2);
                }
            }
        }
    }

    private IEnumerator waitRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    bool RollDice()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < normalSpawnChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool RollDice2()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < extraSpawnChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void repairTile()
    {
        if (destroyedTiles.Count > 0)
        {
            GroundTile randomizedTile = destroyedTiles[Random.Range(0, destroyedTiles.Count)];

            randomizedTile.ResetGroundTile();
        }
    }

    public void activateAutoDestroy()
    {
        foreach (GameObject tile in meteorSpawns)   
        {
            GroundTile tileScript = tile.GetComponent<GroundTile>();

            if (tileScript != null)
            {
                tileScript.enableAutoDestroy();
            }
        }
    }

    public void organizeTiles(GroundTile tile)
    {
        if (tile.isTileDestroyed() && !destroyedTiles.Contains(tile))
        {
            destroyedTiles.Add(tile);
            caculateDiceChances();
        }
        else
        {
            destroyedTiles.Remove(tile);
            caculateDiceChances();
        }
    }

    private void caculateDiceChances()
    {
        normalSpawnChance = (float)destroyedTiles.Count / ((float)meteorSpawns.Count * 2f) * 100;

        extraSpawnChance = (float)destroyedTiles.Count / ((float)meteorSpawns.Count * 4f) * 100;
    }

    public void decreaseSpawnCoolDown(float amount)
    {
        if (spawnCoolDown > 0.2)
        {
            spawnCoolDown -= amount;
        }
    }

    public void PlayDestroyedSound()
    {
        meteorDestroySound.Play();
    }

    public void PlayBonusSound()
    {
        BonusSound.Play();
    }

    public void increaseSpawnCount(int spawnlevel)
    {
        level += spawnlevel;

        if(level >= 1 && level < 5)
        {
            numberOfSpawns += 1;
        }

        if(level >= 5 && level < 10)
        {
            numberOfSpawns += 1;
        }

        if (level >= 10 && level < 15)
        {
            numberOfSpawns += 1;
        }

        if (level >= 15)
        {
            numberOfSpawns += 1;
        }

        if(numberOfSpawns > 7)
        {
            numberOfSpawns = 7;
        }
    }
}
