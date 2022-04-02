using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    [SerializeField]
    private int speed, lifeTime;

    private float TimeStamp;
    // Start is called before the first frame update
    void Start()
    {
        TimeStamp = Time.time + lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.down * speed * Time.deltaTime;

        if(Time.time > TimeStamp)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GroundTile tile = collision.gameObject.GetComponent<GroundTile>();

        if(tile != null)
        {
            tile.DamageGroundTile();
        }

        CharacterController player = collision.gameObject.GetComponent<CharacterController>();

        if(player != null)
        {
            Destroy(player.gameObject);
        }
    }
}