using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> meteorSpawns = new List<GameObject>();

    [SerializeField]
    private int spawnDistance;

    private bool canSpawn;

    [SerializeField]
    private GameObject meteor;

    [SerializeField]
    private float spawnCoolDown;
    private float timeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        meteorSpawns.Clear();

        int numOfChildren = this.transform.childCount;

        for (int i = 0; i < numOfChildren; i++)
        {
            meteorSpawns.Add(this.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
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

        if(meteor != null)
        {
            Instantiate(meteor, spawnlocation, Quaternion.identity);
        }
    }
}
