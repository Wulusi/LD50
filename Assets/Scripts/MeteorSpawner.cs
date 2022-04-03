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

    [SerializeField]
    private float bonusCooldown1, bonusCooldown2;

    private float timeStamp = 0;
    private float bonusTimeStamp1, bonusTimeStamp2;
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
            
            if(bonusTimeStamp1 < Time.time)
            {
                bonusTimeStamp1 = Time.time + bonusCooldown1;
                fallingMeteor.GetComponent<Meteor>().setBonus(1);
            }

            if (bonusTimeStamp2 < Time.time)
            {
                bonusTimeStamp2 = Time.time + bonusCooldown2;
                fallingMeteor.GetComponent<Meteor>().setBonus(2);
            }
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
        }
        else
        {
            destroyedTiles.Remove(tile);
        }
    }
}
