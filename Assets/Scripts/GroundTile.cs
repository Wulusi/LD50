using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{

    [SerializeField]
    private float groundTileHealth;

    [SerializeField]
    private Collider2D blocker;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (groundTileHealth <= 0)
        {
            blocker.isTrigger = true;
        }
        else
        {
            blocker.isTrigger = false;
        }
    }

    public void ResetGroundTile()
    {
        groundTileHealth = 2;
    }

    public void DamageGroundTile()
    {
        groundTileHealth--;
    }
}
