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
            SpawnMeteorsSequence();
        }
    }
    void SpawnMeteorsSequence()
    {
        canSpawn = false;

        int randomSpawn = Random.Range(0, meteorSpawns.Count);

        Vector3 spawnlocation = meteorSpawns[randomSpawn].transform.position + new Vector3(0, spawnDistance);

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
}
