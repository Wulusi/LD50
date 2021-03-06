using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private int speed, lifeTime;

    [SerializeField]
    SpriteRenderer sprite;

    private float TimeStamp;

    [SerializeField]
    private bool containsBonus, containsBonus2;
    // Start is called before the first frame update
    [SerializeField]
    GameObject specialMeteor1, specialMeteor2;
    void Start()
    {
        TimeStamp = Time.time + lifeTime;
        determineIfBonus();
    }
    void determineIfBonus()
    {
        if (containsBonus)
        {
            specialMeteor1.SetActive(true);
        }
        else if (containsBonus2)
        {
            specialMeteor2.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.down * speed * Time.deltaTime;

        if (Time.time > TimeStamp)
        {
            Destroy(this);
        }
    }
    public void setBonus(int bonus)
    {
        if (bonus == 1)
        {
            containsBonus = true;
            sprite.enabled = false;
            specialMeteor1.SetActive(true);
        }
        else
        {
            sprite.enabled = false;
            containsBonus2 = true;
            specialMeteor2.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GroundTile tile = collision.gameObject.GetComponent<GroundTile>();

        if (tile != null)
        {
            tile.DamageGroundTile();
            Destroy(gameObject);
        }

        CharacterController player = collision.gameObject.GetComponent<CharacterController>();

        if (player != null)
        {
            player.killPlayer();
            Destroy(gameObject);
        }
    }
    public void DestroyMeteor()
    {
        MeteorSpawner spawner = FindObjectOfType<MeteorSpawner>();

        if (containsBonus)
        {
            
            spawner.PlayBonusSound();
            spawner.repairTile();
        }

        if(containsBonus2)
        {
            spawner.PlayBonusSound();
            spawner.repairTile();
            spawner.repairTile();
            spawner.repairTile();
        }

        if(!containsBonus || !containsBonus2)
        {
            spawner.PlayDestroyedSound();
        }

        Destroy(gameObject);
    }
}
